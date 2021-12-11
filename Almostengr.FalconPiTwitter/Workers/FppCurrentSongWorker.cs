using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiTwitter.DataTransferObjects;
using Almostengr.FalconPiTwitter.Exceptions;
using Microsoft.Extensions.Logging;
using Tweetinvi;

namespace Almostengr.FalconPiTwitter.Workers
{
    public class FppCurrentSongWorker : BaseWorker, IFppCurrentSongWorker
    {
        private readonly ILogger<FppCurrentSongWorker> _logger;
        private readonly HttpClient _httpClient;

        public FppCurrentSongWorker(ILogger<FppCurrentSongWorker> logger, AppSettings appSettings, ITwitterClient twitterClient) :
            base(logger, appSettings, twitterClient)
        {
            _logger = logger;

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = HostUri;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string previousSong = string.Empty;

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    FalconFppdStatusDto falconFppdStatus = await GetCurrentStatusAsync(_httpClient);

                    if (falconFppdStatus.Current_Song == string.Empty)
                    {
                        throw new FppCurrentSongException();
                    }

                    FalconMediaMetaDto falconMediaMeta =
                        await GetCurrentSongMetaDataAsync(falconFppdStatus.Current_Song);

                    falconMediaMeta.Format.Tags.Title =
                        GetSongTitle(falconFppdStatus.Current_Song_NotFile, falconMediaMeta.Format.Tags.Title);

                    previousSong = await PostCurrentSongAsync(
                        previousSong, falconMediaMeta.Format.Tags.Title,
                        falconMediaMeta.Format.Tags.Artist,
                        falconMediaMeta.Format.Tags.Album,
                        falconFppdStatus.Current_PlayList.Playlist);
                }
                catch (FppCurrentSongException)
                {
                    // _logger.LogWarning("No song is currently playing"); // do nothing
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogError("Are you connected to internet? Is FFPPd running? " + ex.Message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }

                await Task.Delay(TimeSpan.FromSeconds(15));
            }
        }

        public async Task<string> PostCurrentSongAsync(string previousTitle, string currentTitle, string artist, string album, string playlist)
        {
            _logger.LogInformation("Preparing to post current song");

            if (previousTitle == currentTitle)
            {
                _logger.LogDebug("Song title has not changed. Not posting.");
                return previousTitle;
            }

            if (IsIdleOfflineOrTesting(playlist))
            {
                _logger.LogInformation("Show is offline. Not posting song");
                return previousTitle;
            }

            if (string.IsNullOrEmpty(currentTitle))
            {
                _logger.LogInformation("Not posting song as it does not have a title");
                return previousTitle;
            }

            string tweet = string.Concat("Playing \"", currentTitle, "\"");

            if (string.IsNullOrEmpty(artist) == false)
            {
                tweet += " by " + artist;
            }

            tweet += " at " + DateTime.Now.ToShortTimeString();

            tweet += " " + GetRandomHashTag(2);

            await PostTweetAsync(tweet);

            return currentTitle;
        }

        public async Task<FalconMediaMetaDto> GetCurrentSongMetaDataAsync(string currentSong)
        {
            _logger.LogInformation("Getting current song meta data");

            if (string.IsNullOrEmpty(currentSong))
            {
                _logger.LogWarning("No song provided");
                return new FalconMediaMetaDto();
            }

            return await HttpGetAsync<FalconMediaMetaDto>(_httpClient, string.Concat("api/media/", currentSong, "/meta"));
        }

        public string GetSongTitle(string notFileTitle, string tagTitle)
        {
            _logger.LogInformation("Getting song title");
            return string.IsNullOrEmpty(tagTitle) ? notFileTitle : tagTitle;
        }

        public override void Dispose()
        {
            _httpClient.Dispose();
            base.Dispose();
        }

    }
}