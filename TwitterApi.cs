using System;
using System.Threading.Tasks;
using Tweetinvi;
using static Almostengr.FalconPiMonitor.Logger;

namespace Almostengr.FalconPiMonitor
{
    public class TwitterApi
    {
        private readonly string ConsumerKey = Environment.GetEnvironmentVariable("HPC_TWITTER_CONSUMER_KEY");
        private readonly string ConsumerSecret = Environment.GetEnvironmentVariable("HPC_TWITTER_CONSUMER_SECRET");
        private readonly string AccessToken = Environment.GetEnvironmentVariable("HPC_TWITTER_ACCESS_TOKEN");
        private readonly string AccessSecret = Environment.GetEnvironmentVariable("HPC_TWITTER_ACCESS_SECRET");
        private TwitterClient twitterClient { get; set; }

        public TwitterApi()
        {
            twitterClient = new TwitterClient(ConsumerKey, ConsumerSecret, AccessToken, AccessSecret);
        }

        public async Task GetLoggedInUser()
        {
            var user = await twitterClient.Users.GetAuthenticatedUserAsync();
            LogMessage($"Connected to Twitter as {user}");
        }

        public async Task PostTweet(string tweetText)
        {
            if (tweetText.Length > 280)
            {
                LogMessage($"Tweet too long. Truncating. BEFORE: {tweetText}");
                tweetText = tweetText.Substring(0, 280);
            }

#if RELEASE
                var tweet = await twitterClient.Tweets.PublishTweetAsync(tweetText);
#endif

            TwitterMessage(tweetText);
        }
    }
}