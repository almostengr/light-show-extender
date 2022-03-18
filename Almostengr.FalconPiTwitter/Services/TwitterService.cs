using System;
using System.Threading.Tasks;
using Almostengr.FalconPiTwitter.Common;
using Almostengr.FalconPiTwitter.Common.Constants;

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

            tweet = tweet.Trim();
            tweet = tweet.Replace("  ", " ");

            while (tweet.Length > TwitterConstants.TweetCharacterLimit)
            {
                tweet = tweet.Substring(0, tweet.LastIndexOf(" "));
            }

            var response = await _twitterClient.Tweets.PublishTweetAsync(tweet);
            return response.CreatedBy.Name.Length > 0 ? true : false;
        }

        public async Task PostTweetAlarmAsync(string alarmMessage)
        {
            _logger.LogWarning(alarmMessage);
            AlarmCount++;

            if (AlarmCount <= _appSettings.Monitoring.MaxAlarmsPerHour)
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

        public string GetRandomNewYearHashTag()
        {
            string outputTags = string.Empty;
            int numTagsUsed = 0;

            // prevent index out of bounds
            int maxNumHashTags = _appSettings.MaxHashTags > TwitterConstants.NewYearHashTags.Length ?
                TwitterConstants.NewYearHashTags.Length :
                _appSettings.MaxHashTags;

            while (numTagsUsed <= maxNumHashTags)
            {
                string randomTag = TwitterConstants.NewYearHashTags[Random.Next(0, TwitterConstants.NewYearHashTags.Length)];

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
            var user = await _twitterClient.Users.GetAuthenticatedUserAsync();
            _logger.LogInformation($"Authenticated user: {user.Name}");
            return user.Name;
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