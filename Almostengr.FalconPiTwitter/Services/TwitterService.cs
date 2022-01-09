using System;
using System.Threading.Tasks;
using Almostengr.FalconPiTwitter.Constants;
using Almostengr.FalconPiTwitter.Settings;
using Microsoft.Extensions.Logging;
using Tweetinvi;

namespace Almostengr.FalconPiTwitter.Services
{
    public class TwitterService : BaseService, ITwitterService
    {
        private readonly ITwitterClient _twitterClient;
        private readonly ILogger<TwitterService> _logger;
        private readonly AppSettings _appSettings;

        public TwitterService(ILogger<TwitterService> logger, AppSettings appSettings,
            ITwitterClient twitterClient) : base(logger)
        {
            _logger = logger;
            _twitterClient = twitterClient;
            _appSettings = appSettings;
        }

        public async Task<bool> PostTweetAsync(string tweet)
        {
            if (string.IsNullOrEmpty(tweet))
            {
                _logger.LogWarning("Nothing to tweet");
                return false;
            }

            tweet = tweet.Trim().Replace("  ", " ");

            while (tweet.Length > TwitterConstants.TweetCharacterLimit)
            {
                tweet = tweet.Substring(0, tweet.LastIndexOf(" "));
            }

            if (_appSettings.DemoMode)
            {
                _logger.LogWarning("Demo mode active. TWEET: {0}", tweet);
                return true;
            }

            try
            {
                var response = await _twitterClient.Tweets.PublishTweetAsync(tweet);
                return response.CreatedBy.Name.Length > 0 ? true : false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        public async Task PostTweetAlarmAsync(string alarmMessage, int alarmCount = 0)
        {
            if (string.IsNullOrEmpty(alarmMessage))
            {
                return;
            }

            _logger.LogWarning(alarmMessage);

            if (alarmCount <= _appSettings.Monitoring.MaxAlarmsPerHour)
            {
                string users = string.Empty;

                foreach (string user in _appSettings.Monitoring.AlarmUsernames)
                {
                    users += user + " ";
                }

                await PostTweetAsync(users + alarmMessage);
            }
        }

        public string GetRandomChristmasHashTag()
        {
            string outputTags = string.Empty;
            int numTagsUsed = 0;

            // prevent index out of bounds
            int maxNumHashTags = _appSettings.MaxHashTags > TwitterConstants.ChristmasHashTags.Length ?
                TwitterConstants.ChristmasHashTags.Length :
                _appSettings.MaxHashTags;

            while (numTagsUsed <= maxNumHashTags)
            {
                string randomTag = TwitterConstants.ChristmasHashTags[Random.Next(0, TwitterConstants.ChristmasHashTags.Length)];

                if (outputTags.Contains(randomTag) == false)
                {
                    outputTags += randomTag + " ";
                    numTagsUsed++;
                }
            }

            return outputTags;
        }

        public async Task<string> GetAuthenticatedUserAsync()
        {
            try
            {
                var user = await _twitterClient.Users.GetAuthenticatedUserAsync();
                _logger.LogInformation($"Authenticated user: {user.Name}");
                return user.Name;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return string.Empty;
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

    }
}