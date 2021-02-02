using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiMonitor.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Tweetinvi;

namespace Almostengr.FalconPiMonitor.Services
{
    public class BaseService : BackgroundService
    {
        public readonly ILogger<BaseService> logger;
        public readonly IConfiguration config;
        public AppSettings AppSettings;
        public HttpClient HttpClient;
        public TwitterClient TwitterClient;
        public int ExecuteDelaySeconds = 5;

        public BaseService(ILogger<BaseService> logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.config = configuration;
            this.AppSettings = new AppSettings();
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting service. Exit program by pressing Ctrl+C");

            ConfigurationBinder.Bind(config, AppSettings);

            HttpClient = new HttpClient();
            TwitterClient = new TwitterClient(
                AppSettings.Twitter.ConsumerKey,
                AppSettings.Twitter.ConsumerSecret,
                AppSettings.Twitter.AccessToken,
                AppSettings.Twitter.AccessSecret);

            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await GetTwitterUsername();

            while (!stoppingToken.IsCancellationRequested)
            {
                // do nothing
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Shutting down");

            HttpClient.Dispose();
            return base.StopAsync(cancellationToken);
        }

        public async Task GetTwitterUsername()
        {
            var user = await TwitterClient.Users.GetAuthenticatedUserAsync();
            logger.LogInformation("Connected to Twitter as {user}", user.ScreenName);
        }

        public async Task PostTweet(string tweetText, bool sendTestTweet = false)
        {
            if (tweetText.Length > 280)
            {
                logger.LogWarning("Tweet is too long. Truncating. BEFORE: {tweetText}", tweetText);
                tweetText = tweetText.Substring(0, 280);
            }

            if (sendTestTweet == false)
            {
                var tweet = await TwitterClient.Tweets.PublishTweetAsync(tweetText);
                logger.LogInformation("TWEETED: {tweetText}", tweetText);
            }
            else
            {
                logger.LogInformation("TWEETED [Test Mode]: {tweetText}", tweetText);
            }
        }

        public async Task<FalconFppdStatus> GetCurrentStatus()
        {
            HttpResponseMessage response =
                await HttpClient.GetAsync(string.Concat(AppSettings.FalconPiPlayer.FalconUri, "fppd/status"));

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<FalconFppdStatus>(response.Content.ReadAsStringAsync().Result);
            }
            else
            {
                throw new System.Exception(response.ReasonPhrase);
            }
        }

        public bool IsTestingOrOfflinePlaylist(string playlistName)
        {
            playlistName = playlistName.ToLower();

            if (playlistName.Contains("offline") || playlistName.Contains("test"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    } // end class
}