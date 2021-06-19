using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiTwitter.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Tweetinvi;

namespace Almostengr.FalconPiTwitter.Workers
{
    public abstract class BaseWorker : BackgroundService, IBaseWorker
    {
        private readonly AppSettings _appSettings;
        private readonly ITwitterClient _twitterClient;
        private readonly ILogger<BaseWorker> _logger;

        internal readonly Uri HostUri = new Uri("http://localhost/");
        internal const int TweetMaxLength = 280;

        public BaseWorker(ILogger<BaseWorker> logger, AppSettings appSettings, ITwitterClient twitterClient)
        {
            _appSettings = appSettings;
            _twitterClient = twitterClient;
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            var response = _twitterClient.Users.GetAuthenticatedUserAsync();
            _logger.LogInformation(string.Concat("Connected to twitter as ", response.Result.Name));
            return base.StartAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new System.NotImplementedException();
        }

        public virtual async Task<FalconFppdStatus> GetCurrentStatusAsync(HttpClient httpClient)
        {
            return await HttpGetAsync<FalconFppdStatus>(httpClient, "api/fppd/status");
        }

        public async Task<T> HttpGetAsync<T>(HttpClient httpClient, string route) where T : ModelBase
        {
            HttpResponseMessage response = await httpClient.GetAsync(route);

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            }
            else
            {
                throw new System.Exception(response.ReasonPhrase);
            }
        }

        public async Task<bool> PostTweetAsync(string tweet)
        {
            if (string.IsNullOrEmpty(tweet))
            {
                _logger.LogWarning("Nothing to tweet.");
                return false;
            }

            tweet = tweet.Trim();

            // trim the tweet between words if it is too long
            while(tweet.Length > TweetMaxLength)
            {
                tweet = tweet.Substring(0, tweet.LastIndexOf(" "));
            }

            _logger.LogInformation("Tweeting: " + tweet);

#if RELEASE
            var response = await _twitterClient.Tweets.PublishTweetAsync(tweet);
            _logger.LogInformation("Sent tweet at: " + response.CreatedAt.ToString());
            return response.CreatedBy.Name.Length > 0 ? true : false;
#else
            await Task.Delay(TimeSpan.FromSeconds(1));
            _logger.LogInformation("Sent testing tweet");
            return true;
#endif
        }

    }
}