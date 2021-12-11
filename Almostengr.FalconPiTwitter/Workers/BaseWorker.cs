using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiTwitter.DataTransferObjects;
using Almostengr.FalconPiTwitter.Settings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Tweetinvi;

namespace Almostengr.FalconPiTwitter.Workers
{
    public abstract class BaseWorker : BackgroundService, IBaseWorker
    {
        private readonly ITwitterClient _twitterClient;
        private readonly ILogger<BaseWorker> _logger;

        internal readonly Uri HostUri;
        internal const int TweetMaxLength = 280;
        internal readonly Random Random;

        public BaseWorker(ILogger<BaseWorker> logger, AppSettings appSettings, ITwitterClient twitterClient)
        {
            _twitterClient = twitterClient;
            _logger = logger;
            Random = new Random();

            HostUri = new Uri("http://localhost/");
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

        public virtual async Task<FalconFppdStatusDto> GetCurrentStatusAsync(HttpClient httpClient)
        {
            return await HttpGetAsync<FalconFppdStatusDto>(httpClient, "api/fppd/status");
        }

        public async Task<T> HttpGetAsync<T>(HttpClient httpClient, string route) where T : BaseDto
        {
            HttpResponseMessage response = await httpClient.GetAsync(route);

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
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

            // trim the tweet between words if it is too long
            while (tweet.Length > TweetMaxLength)
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

        public string GetRandomHashTag(int count = 1)
        {
            string[] hashTags = {
                "#LightShow", "#AnimatedLights", "#LedLighting",
                "#ChristmasLightShow", "#ChristmasLights", "#Christmas", "#Christmas", "#ChristmasSeason",
                "#ChristmasTime", "#ChristmasDecorations", "#ChristmasSpirit", "#ChristmasMagic",
                "#ChristmasFun", $"#Christmas{DateTime.Now.Year}", "#MerryChristmas", "#ChristmasMusic",
                "#ChristmasLighting",
                "#HolidayLightShow", "#HolidayLightShows", "#HolidayLights", "#HappyHolidays",
                "#HolidayLighting"
                };
            string outputTags = string.Empty;
            int tagsUsed = 0;

            count = count > hashTags.Length ? hashTags.Length : count; // prevent index out of bounds

            while (tagsUsed <= count)
            {
                string randomTag = hashTags[Random.Next(0, hashTags.Length)];

                if (outputTags.Contains(randomTag) == false)
                {
                    outputTags += randomTag + " ";
                    tagsUsed++;
                }
            }

            return outputTags;
        }

        public bool IsPlaylistIdleOfflineOrTesting(FalconFppdStatusDto status)
        {
            string playlistName = status.Current_PlayList.Playlist.ToLower();

            return (status == null || 
                (playlistName == "idle" || playlistName == "testing" || playlistName == "offline"));
        }

        public bool IsIdleOfflineOrTesting(string input)
        {
            if (input.ToLower().Contains("offline") || input.ToLower().Contains("test") || string.IsNullOrEmpty(input))
            {
                _logger.LogInformation("Show is idle, testing, or offline");
                return true;
            }

            return false;
        }
    }
}