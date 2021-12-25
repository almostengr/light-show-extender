using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiTwitter.DataTransferObjects;
using Almostengr.FalconPiTwitter.Settings;
using Microsoft.Extensions.Logging;
using Tweetinvi;
using Almostengr.FalconPiTwitter.Constants;

namespace Almostengr.FalconPiTwitter.Workers
{
    public class FppCurrentSongWorker : BaseWorker
    {
        private readonly ILogger<FppCurrentSongWorker> _logger;
        private readonly AppSettings _appSettings;
        private readonly HttpClient _httpClient;

        public FppCurrentSongWorker(ILogger<FppCurrentSongWorker> logger, AppSettings appSettings, ITwitterClient twitterClient) :
            base(logger, appSettings, twitterClient)
        {
            _logger = logger;
            _appSettings = appSettings;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = AppConstants.Localhost;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string previousSong = string.Empty;

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(DelaySeconds.Short), stoppingToken);

                try
                {
                    FalconFppdStatusDto falconFppdStatus = await GetFppdStatusAsync(_httpClient);

                    if (falconFppdStatus.Mode_Name == FppMode.Remote)
                    {
                        _logger.LogDebug("This is remote instance of FPP");
                        break;
                    }

                    if (falconFppdStatus.Current_Song == string.Empty)
                    {
                        _logger.LogDebug("No song is currently playling");
                        continue;
                    }

                    FalconMediaMetaDto falconMediaMeta =
                        await GetCurrentSongMetaDataAsync(falconFppdStatus.Current_Song);

                    _logger.LogDebug("Getting song title");
                    falconMediaMeta.Format.Tags.Title =
                        string.IsNullOrEmpty(falconMediaMeta.Format.Tags.Title) ?
                        falconFppdStatus.Current_Song_NotFile :
                        falconMediaMeta.Format.Tags.Title;

                    previousSong = await PostCurrentSongAsync(
                        previousSong, falconMediaMeta.Format.Tags.Title,
                        falconMediaMeta.Format.Tags.Artist,
                        falconFppdStatus.Current_PlayList.Playlist);
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogError(ExceptionMessage.NoInternetConnection + ex.Message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }
            }
        }

        public async Task<string> PostCurrentSongAsync(string previousTitle, string currentTitle, string artist, string playlist)
        {
            _logger.LogDebug("Preparing to post current song");

            if (previousTitle == currentTitle || string.IsNullOrEmpty(currentTitle))
            {
                _logger.LogDebug("Not posting song information");
                return previousTitle;
            }

            string tweet = $"Playing \"{currentTitle}\"";

            if (string.IsNullOrEmpty(artist) == false)
            {
                tweet += $" by {artist}";
            }

            tweet += $" at {DateTime.Now.ToShortTimeString()}";
            tweet += $" {GetRandomChristmasHashTag()}";

            await PostTweetAsync(tweet);

            return currentTitle;
        }

        public async Task<FalconMediaMetaDto> GetCurrentSongMetaDataAsync(string currentSong)
        {
            _logger.LogDebug("Getting current song meta data");

            if (string.IsNullOrEmpty(currentSong))
            {
                _logger.LogWarning("No song provided");
                return new FalconMediaMetaDto();
            }

            return await HttpGetAsync<FalconMediaMetaDto>(_httpClient, $"api/media/{currentSong}/meta");
        }

        public override void Dispose()
        {
            _httpClient.Dispose();
            base.Dispose();
        }

    }
}