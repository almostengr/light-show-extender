using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiMonitor.Models;
using Microsoft.Extensions.Logging;
using Tweetinvi;

namespace Almostengr.FalconPiTwitter.Workers
{
    public class FppCurrentSongWorker : BaseWorker, IFppCurrentSongWorker
    {
        private readonly ILogger<FppCurrentSongWorker> _logger;
        private readonly ITwitterClient _twitterClient;
        private readonly AppSettings _appSettings;

        public FppCurrentSongWorker(
            ILogger<FppCurrentSongWorker> logger, AppSettings appSettings, HttpClient httpClient,
            ITwitterClient twitterClient) :
            base(logger, appSettings, httpClient, twitterClient)
        {
            _logger = logger;
            _appSettings = appSettings;
            _twitterClient = twitterClient;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _httpClient.BaseAddress = new Uri(_appSettings.FalconPiPlayerUrl);
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting Fpp Current Song Worker");

            string previousSong = string.Empty;

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (previousSong == string.Empty)
                    {
                        await GetTwitterUsername();
                    }

                    FalconFppdStatus falconFppdStatus = await GetCurrentStatus();
                    FalconMediaMeta falconMediaMeta = await GetCurrentSongMetaData(falconFppdStatus.Current_Song);

                    falconMediaMeta.Format.Tags.Title =
                        GetSongTitle(falconFppdStatus.Current_Song_NotFile, falconMediaMeta.Format.Tags.Title);

                    previousSong = await PostCurrentSong(
                        previousSong, falconMediaMeta.Format.Tags.Title,
                        falconMediaMeta.Format.Tags.Artist,
                        falconMediaMeta.Format.Tags.Album,
                        falconFppdStatus.Current_PlayList.Playlist);

                    await Task.Delay(TimeSpan.FromSeconds(15));
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

        public async Task<string> PostCurrentSong(string previousTitle, string currentTitle, string artist, string album, string playlist)
        {
            int tweetLimit = 280 - 14;

            if (previousTitle == currentTitle)
            {
                return previousTitle;
            }

            playlist = playlist.ToLower();
            if (playlist.Contains("offline") || playlist.Contains("testing"))
            {
                _logger.LogInformation("Show is offline. Not posting song");
                return currentTitle;
            }

            string tweet = "Playing ";

            if (string.IsNullOrEmpty(currentTitle) == false)
            {
                tweet = string.Concat(tweet, "\"", currentTitle, "\"");
            }
            else
            {
                _logger.LogInformation("Not tweeting song as it does not have a title");
                return previousTitle;
            }

            if (string.IsNullOrEmpty(artist) && tweet.Length < tweetLimit)
            {
                tweet = string.Concat(tweet, " by ", artist);
            }

            if (string.IsNullOrEmpty(album) && tweet.Length < tweetLimit)
            {
                tweet = string.Concat(tweet, " (", album, ")");
            }

            tweet = string.Concat(tweet, " at ", DateTime.Now.ToLongTimeString());

            var tweetResult = await _twitterClient.Tweets.PublishTweetAsync(tweet);

            _logger.LogInformation("Posted song update. Result " + tweetResult.Id);
            return currentTitle;
        }

        public async Task<FalconMediaMeta> GetCurrentSongMetaData(string current_Song)
        {
            return await GetAsync<FalconMediaMeta>(string.Concat("media/", current_Song, "/meta"));
        }

        public async Task<FalconFppdStatus> GetCurrentStatus()
        {
            return await GetAsync<FalconFppdStatus>("fppd/status");
        }

        public async Task GetTwitterUsername()
        {
            var user = await _twitterClient.Users.GetAuthenticatedUserAsync();
            _logger.LogInformation("Connected to Twitter as {user}", user);
        }

        public string GetSongTitle(string notFileTitle, string tagTitle)
        {
            if (string.IsNullOrEmpty(tagTitle))
            {
                return notFileTitle;
            }

            return tagTitle;
        }

    }
}