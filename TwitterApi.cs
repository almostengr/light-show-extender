using System;
using System.Threading.Tasks;
using Tweetinvi;
using static Almostengr.FalconPiMonitor.Logger;

namespace Almostengr.FalconPiMonitor
{
    public class TwitterApi
    {
        // private string ConsumerKey { get; set; }
        // private string ConsumerSecret { get; set; }
        // private string AccessToken { get; set; }
        // private string AccessSecret { get; set; }
        private TwitterClient twitterClient { get; set; }

        public TwitterApi(string consumerKey, string consumerSecret, string accessToken, string accessSecret)
        {
            twitterClient = new TwitterClient(consumerKey, consumerSecret, accessToken, accessSecret);
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