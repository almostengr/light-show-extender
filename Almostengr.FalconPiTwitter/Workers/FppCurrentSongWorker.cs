using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiTwitter.Models;
using Almostengr.FalconPiTwitter.Exceptions;
using Microsoft.Extensions.Logging;
using Tweetinvi;

namespace Almostengr.FalconPiTwitter.Workers
{
    public class FppCurrentSongWorker : BaseWorker, IFppCurrentSongWorker
    {
        private readonly ILogger<FppCurrentSongWorker> _logger;
        private readonly ITwitterClient _twitterClient;
        private readonly AppSettings _appSettings;
        private readonly HttpClient _httpClient;

        public FppCurrentSongWorker(ILogger<FppCurrentSongWorker> logger, AppSettings appSettings, ITwitterClient twitterClient) :
            base(logger, appSettings, twitterClient)
        {
            _logger = logger;
            _appSettings = appSettings;
            _twitterClient = twitterClient;

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
                    FalconFppdStatus falconFppdStatus = await GetCurrentStatusAsync(_httpClient);

                    if (falconFppdStatus.Current_Song == string.Empty)
                    {
                        throw new FppCurrentSongException();
                    }

                    FalconMediaMeta falconMediaMeta =
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
                    _logger.LogWarning("No song is currently playing");
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
                    _logger.LogError(ex, string.Concat(ex.GetType(), ex.Message));
                }

                await Task.Delay(TimeSpan.FromSeconds(15));
            }
        }

        public async Task<string> PostCurrentSongAsync(string previousTitle, string currentTitle, string artist, string album, string playlist)
        {
            _logger.LogInformation("Preparing to post current song");

            int tweetLimit = TweetMaxLength - 14; // 280 - 14;

            if (previousTitle == currentTitle)
            {
                _logger.LogInformation("Song title has not changed. Not posting.");
                return previousTitle;
            }

            playlist = playlist.ToLower(); // remove case sensitivity before comparing
            if (playlist.Contains("offline") || playlist.Contains("testing"))
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

            if (string.IsNullOrEmpty(artist) && tweet.Length < tweetLimit)
            {
                tweet += " by " + artist;
            }

            if (tweet.Length < tweetLimit)
            {
                tweet += " " + GetRandomHashTag(2);
            }

            tweet = string.Concat(tweet, " at ", DateTime.Now.ToLongTimeString());

            await PostTweetAsync(tweet);

            return currentTitle;
        }

        public async Task<FalconMediaMeta> GetCurrentSongMetaDataAsync(string currentSong)
        {
            _logger.LogInformation("Getting current song meta data");

            if (string.IsNullOrEmpty(currentSong))
            {
                _logger.LogWarning("No song provided");
                return new FalconMediaMeta();
            }

            return await HttpGetAsync<FalconMediaMeta>(_httpClient, string.Concat("api/media/", currentSong, "/meta"));
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