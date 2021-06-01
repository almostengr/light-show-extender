using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiMonitor.Models;
using Microsoft.Extensions.Logging;
using Tweetinvi;

namespace Almostengr.FalconPiTwitter.Workers
{
    public class MockFppCurrentSongWorker : BaseWorker, IFppCurrentSongWorker
    {
        private readonly ILogger<MockFppCurrentSongWorker> _logger;
        private readonly AppSettings _appsettings;
        private HttpClient _httpClient;
        private ITwitterClient _twitterClient;

        public MockFppCurrentSongWorker(
            ILogger<MockFppCurrentSongWorker> logger,
            AppSettings appSettings,
            ITwitterClient twitterClient) :
            base(logger, appSettings, twitterClient)
        {
            _logger = logger;
            _appsettings = appSettings;
            _twitterClient = twitterClient;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting Fpp current song worker");
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost");
            return base.StartAsync(cancellationToken);
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
                    FalconFppdStatus falconFppdStatus = await GetCurrentStatusAsync(httpClient);
                    FalconMediaMeta falconMediaMeta = await GetCurrentSongMetaDataAsync(falconFppdStatus.Current_Song);

                    falconMediaMeta.Format.Tags.Title =
                        GetSongTitle(falconFppdStatus.Current_Song_NotFile, falconMediaMeta.Format.Tags.Title);

                    previousSong = await PostCurrentSongAsync(
                        previousSong, falconMediaMeta.Format.Tags.Title,
                        falconMediaMeta.Format.Tags.Artist,
                        falconMediaMeta.Format.Tags.Album,
                        falconFppdStatus.Current_PlayList.Playlist);

                    await Task.Delay(TimeSpan.FromSeconds(5));
                }
                catch (NullReferenceException ex)
                {
                    _logger.LogError(string.Concat("Null Exception occurred: ", ex.Message));
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogError(string.Concat("Are you connected to internet? HttpRequest Exception occured: ", ex.Message));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
        }

        private static Random random = new Random();
        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public override async Task<FalconFppdStatus> GetCurrentStatusAsync(HttpClient httpClient)
        {
            FalconFppdStatus falconFppdStatus = new FalconFppdStatus();
            falconFppdStatus.Current_Song = RandomString(25);
            falconFppdStatus.Current_PlayList = new FalconFppdStatusCurrentPlayList();
            falconFppdStatus.Current_PlayList.Playlist = RandomString(40);

            return await Task.FromResult(falconFppdStatus);
        }

        public Task<FalconMediaMeta> GetCurrentSongMetaDataAsync(string currentSong)
        {
            _logger.LogInformation("Getting current song meta data");

            var falconMediaMeta = new FalconMediaMeta();
            falconMediaMeta.Format = new FalconMediaMetaFormat();
            falconMediaMeta.Format.Tags = new FalconMediaMetaFormatTags();
            falconMediaMeta.Format.Tags.Title = RandomString(15);
            falconMediaMeta.Format.Tags.Artist = RandomString(20);
            falconMediaMeta.Format.Tags.Album = RandomString(5);

            return Task.FromResult(falconMediaMeta);
        }

        public string GetSongTitle(string notFileTitle, string tagTitle)
        {
            _logger.LogInformation("Getting song title");
            return string.IsNullOrEmpty(tagTitle) ? notFileTitle : tagTitle;
        }

        public Task<string> PostCurrentSongAsync(string previousTitle, string currentTitle, string artist, string album, string playlist)
        {
            _logger.LogInformation("Posting current song");

            int tweetLimit = 266; // 280 - 14;

            if (previousTitle == currentTitle)
            {
                return Task.FromResult(previousTitle);
            }

            playlist = playlist.ToLower();
            if (playlist.Contains("offline") || playlist.Contains("testing"))
            {
                _logger.LogInformation("Show is offline. Not posting song");
                return Task.FromResult(previousTitle);
            }

            if (string.IsNullOrEmpty(currentTitle))
            {
                _logger.LogInformation("Not tweeting song as it does not have a title");
                return Task.FromResult(previousTitle);
            }

            string tweet = string.Concat("Playing ", "\"", currentTitle, "\"");

            if (string.IsNullOrEmpty(artist) && tweet.Length < tweetLimit)
            {
                tweet = string.Concat(tweet, " by ", artist);
            }

            if (string.IsNullOrEmpty(album) && tweet.Length < tweetLimit)
            {
                tweet = string.Concat(tweet, " (", album, ")");
            }

            tweet = string.Concat(tweet, " at ", DateTime.Now.ToLongTimeString());

            _logger.LogInformation("Posted song update. Result posted");
            return Task.FromResult(currentTitle);
        }
    }
}