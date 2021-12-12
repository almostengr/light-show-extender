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
    public class FppCurrentSongWorker : BaseWorker, IFppCurrentSongWorker
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
                try
                {
                    FalconFppdStatusDto falconFppdStatus = await GetFppdStatusAsync(_httpClient);

                    if (falconFppdStatus.Mode_Name == FppMode.Remote)
                    {
                        break;
                    }

                    if (falconFppdStatus.Current_Song == string.Empty)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(DelaySeconds.Short), stoppingToken);
                        continue;
                    }

                    FalconMediaMetaDto falconMediaMeta =
                        await GetCurrentSongMetaDataAsync(falconFppdStatus.Current_Song);

                    falconMediaMeta.Format.Tags.Title =
                        GetSongTitle(falconFppdStatus.Current_Song_NotFile, falconMediaMeta.Format.Tags.Title);

                    previousSong = await PostCurrentSongAsync(
                        previousSong, falconMediaMeta.Format.Tags.Title,
                        falconMediaMeta.Format.Tags.Artist,
                        falconFppdStatus.Current_PlayList.Playlist);
                }
                // catch (FppCurrentSongException)
                // {
                //     // _logger.LogWarning("No song is currently playing"); // do nothing
                // }
                catch (HttpRequestException ex)
                {
                    _logger.LogError(ExceptionMessage.NoInternetConnection + ex.Message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }

                await Task.Delay(TimeSpan.FromSeconds(DelaySeconds.Short), stoppingToken);
            }
        }

        public async Task<string> PostCurrentSongAsync(string previousTitle, string currentTitle, string artist, string playlist)
        {
            _logger.LogDebug("Preparing to post current song");

            if (previousTitle == currentTitle || string.IsNullOrEmpty(currentTitle))
            {
                _logger.LogInformation("Not posting song information");
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

            return await GetMediaMetaAsync(_httpClient, currentSong);
        }

        public async Task<FalconMediaMetaDto> GetMediaMetaAsync(HttpClient httpClient, string musicFilename)
        {
            return await HttpGetAsync<FalconMediaMetaDto>(httpClient, $"api/media/{musicFilename}/meta");
        }
        
        public string GetSongTitle(string notFileTitle, string tagTitle)
        {
            _logger.LogDebug("Getting song title");
            return string.IsNullOrEmpty(tagTitle) ? notFileTitle : tagTitle;
        }

        public override void Dispose()
        {
            _httpClient.Dispose();
            base.Dispose();
        }

    }
}