using Almostengr.FalconPiTwitter.Common.Constants;
using Almostengr.FalconPiTwitter.Common.Extensions;
using Microsoft.Extensions.Logging;
using Tweetinvi;

namespace Almostengr.FalconPiTwitter.Common.Services
{
    public class TwitterService : ITwitterService
    {
        private readonly ITwitterClient _twitterClient;
        private readonly ILogger<TwitterService> _logger;
        private readonly AppSettings _appSettings;
        private int AlarmCount = 0;
        private readonly Random _random = new Random();

        public TwitterService(ILogger<TwitterService> logger, AppSettings appSettings,
            ITwitterClient twitterClient)
        {
            _logger = logger;
            _twitterClient = twitterClient;
            _appSettings = appSettings;
        }

        public async Task<bool> PostTweetAsync(string tweet)
        {
            if (tweet.IsNullOrEmpty())
            {
                _logger.LogWarning("Nothing to tweet");
                return false;
            }

            tweet = tweet.Trim().Replace("  ", " ");

            while (tweet.Length > TwitterConstants.TweetCharacterLimit)
            {
                tweet = tweet.Substring(0, tweet.LastIndexOf(" "));
            }

            _logger.LogInformation($"Sending tweet: {tweet}");
            var response = await _twitterClient.Tweets.PublishTweetAsync(tweet);
            return response.CreatedBy.Name.Length > 0 ? true : false;
        }

        public async Task PostTweetAlarmAsync(string alarmMessage)
        {
            if (alarmMessage.IsNullOrEmpty())
            {
                return;
            }

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

        public string GetRandomChristmasHashTags()
        {
            string outputTags = string.Empty;
            int numTagsUsed = 0;

            int maxNumHashTags = _appSettings.MaxHashTags > TwitterConstants.ChristmasHashTags.Length ?
                TwitterConstants.ChristmasHashTags.Length :
                _appSettings.MaxHashTags;

            while (numTagsUsed <= maxNumHashTags)
            {
                string randomTag = TwitterConstants.ChristmasHashTags[_random.Next(0, TwitterConstants.ChristmasHashTags.Length)];

                if (outputTags.Contains(randomTag) == false)
                {
                    outputTags += randomTag + " ";
                    numTagsUsed++;
                }
            }

            return outputTags;
        }

        public string GetRandomNewYearHashTags()
        {
            string outputTags = string.Empty;
            int numTagsUsed = 0;

            int maxNumHashTags = _appSettings.MaxHashTags > TwitterConstants.NewYearHashTags.Length ?
                TwitterConstants.NewYearHashTags.Length :
                _appSettings.MaxHashTags;

            while (numTagsUsed <= maxNumHashTags)
            {
                string randomTag = TwitterConstants.NewYearHashTags[_random.Next(0, TwitterConstants.NewYearHashTags.Length)];

                if (outputTags.Contains(randomTag) == false)
                {
                    outputTags += randomTag + " ";
                    numTagsUsed++;
                }
            }

            return outputTags;
        }

        public async Task<string> PostCurrentSongAsync(string currentTitle, string artist)
        {
            string tweet = $"Playing \"{currentTitle}\"";

            if (artist.IsNullOrEmpty() == false)
            {
                tweet += $" by {artist}";
            }

            tweet += $" at {DateTime.Now.ToShortTimeString()} {GetRandomChristmasHashTags()}";

            await PostTweetAsync(tweet);

            return currentTitle;
        }

    }
}