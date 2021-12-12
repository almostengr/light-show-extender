using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiTwitter.Constants;
using Almostengr.FalconPiTwitter.DataTransferObjects;
using Almostengr.FalconPiTwitter.Settings;
using Microsoft.Extensions.Logging;
using Tweetinvi;

namespace Almostengr.FalconPiTwitter.Workers
{
    public class MockFppCurrentSongWorker : MockBaseWorker, IFppCurrentSongWorker
    {
        private readonly ILogger<MockFppCurrentSongWorker> _logger;
        private readonly AppSettings _appsettings;
        private readonly Random _random = new Random();

        public MockFppCurrentSongWorker(
            ILogger<MockFppCurrentSongWorker> logger,
            AppSettings appSettings) :
            base(logger)
        {
            _logger = logger;
            _appsettings = appSettings;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping Fpp current song worker");
            return base.StopAsync(cancellationToken);
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string previousSong = string.Empty;
            HttpClient httpClient = new HttpClient();

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    FalconFppdStatusDto falconFppdStatus = await GetFppdStatusAsync(httpClient);
                    FalconMediaMetaDto falconMediaMeta = await GetCurrentSongMetaDataAsync(falconFppdStatus.Current_Song);

                    falconMediaMeta.Format.Tags.Title =
                        GetSongTitle(falconFppdStatus.Current_Song_NotFile, falconMediaMeta.Format.Tags.Title);

                    previousSong = await PostCurrentSongAsync(
                        previousSong, falconMediaMeta.Format.Tags.Title,
                        falconMediaMeta.Format.Tags.Artist,
                        falconFppdStatus.Current_PlayList.Playlist);

                    await Task.Delay(TimeSpan.FromSeconds(5));
                }
                catch (NullReferenceException ex)
                {
                    _logger.LogError(string.Concat(ExceptionMessage.NullReference, ex.Message));
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogError(string.Concat(ExceptionMessage.NoInternetConnection, ex.Message));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
        }

        private string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[_random.Next(s.Length)]).ToArray());
        }

        public override async Task<FalconFppdStatusDto> GetFppdStatusAsync(HttpClient httpClient)
        {
            FalconFppdStatusDto falconFppdStatus = new FalconFppdStatusDto();
            falconFppdStatus.Current_Song = RandomString(25);
            falconFppdStatus.Current_PlayList = new FalconFppdStatusCurrentPlayList();
            falconFppdStatus.Current_PlayList.Playlist = RandomString(40);

            return await Task.FromResult(falconFppdStatus);
        }

        public Task<FalconMediaMetaDto> GetCurrentSongMetaDataAsync(string currentSong)
        {
            _logger.LogInformation("Getting current song meta data");

            var falconMediaMeta = new FalconMediaMetaDto();
            falconMediaMeta.Format = new FalconMediaMetaFormat();
            falconMediaMeta.Format.Tags = new FalconMediaMetaFormatTags();
            falconMediaMeta.Format.Tags.Title = RandomString(15);
            falconMediaMeta.Format.Tags.Artist = RandomString(20);

            return Task.FromResult(falconMediaMeta);
        }

        public string GetSongTitle(string notFileTitle, string tagTitle)
        {
            _logger.LogInformation("Getting song title");
            return string.IsNullOrEmpty(tagTitle) ? notFileTitle : tagTitle;
        }

        public Task<string> PostCurrentSongAsync(string previousTitle, string currentTitle, string artist, string playlist)
        {
            _logger.LogInformation("Posting current song");

            if (previousTitle == currentTitle)
            {
                return Task.FromResult(previousTitle);
            }

            playlist = playlist.ToLower();
            if (playlist.Contains(PlaylistIgnoreName.Offline) || playlist.Contains(PlaylistIgnoreName.Testing))
            {
                _logger.LogInformation("Show is offline. Not posting song");
                return Task.FromResult(previousTitle);
            }

            if (string.IsNullOrEmpty(currentTitle))
            {
                _logger.LogInformation("Not tweeting song as it does not have a title");
                return Task.FromResult(previousTitle);
            }

            string tweet = $"Playing \"{currentTitle}\"";

            if (string.IsNullOrEmpty(artist) && tweet.Length < TwitterConstants.TweetCharacterLimit)
            {
                tweet += $" by {artist}";
            }

            tweet += $" at {DateTime.Now.ToLongTimeString()}";

            _logger.LogInformation("Posted song update. Result posted");
            return Task.FromResult(currentTitle);
        }
    }
}