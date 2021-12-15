using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiTwitter.Constants;
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
        private readonly AppSettings _appSettings;
        internal readonly Random Random;
        internal int AlarmCount = 0;

        public BaseWorker(ILogger<BaseWorker> logger, AppSettings appSettings, ITwitterClient twitterClient)
        {
            _twitterClient = twitterClient;
            _logger = logger;
            _appSettings = appSettings;
            Random = new Random();
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            var response = _twitterClient.Users.GetAuthenticatedUserAsync();
            _logger.LogInformation($"Connected to twitter as {response.Result.Name}");
            return base.StartAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new System.NotImplementedException();
        }

        public virtual async Task<FalconFppdStatusDto> GetFppdStatusAsync(HttpClient httpClient)
        {
            return await HttpGetAsync<FalconFppdStatusDto>(httpClient, "api/fppd/status");
        }

        public async Task<FalconFppdMultiSyncSystemsDto> GetMultiSyncStatusAsync(HttpClient httpClient)
        {
            return await HttpGetAsync<FalconFppdMultiSyncSystemsDto>(httpClient, "api/fppd/multiSyncSystems");
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

            while (tweet.Length > TwitterConstants.TweetCharacterLimit)
            {
                tweet = tweet.Substring(0, tweet.LastIndexOf(" "));
            }

#if RELEASE
            var response = await _twitterClient.Tweets.PublishTweetAsync(tweet);
            return response.CreatedBy.Name.Length > 0 ? true : false;
#else
            await Task.Delay(TimeSpan.FromSeconds(1));
            _logger.LogInformation($"Sent testing tweet: {tweet}");
            return true;
#endif
        }

        public async Task PostTweetAlarmAsync(string alarmMessage)
        {
            _logger.LogWarning(alarmMessage);

            if (AlarmCount <= _appSettings.Monitor.MaxAlarmsPerHour)
            {
                string users = string.Empty;

                foreach (string user in _appSettings.Monitor.AlarmUsernames)
                {
                    users += user + " ";
                }

                await PostTweetAsync(users + " " + alarmMessage);
            }
            
            AlarmCount++;
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

        public bool IsPlaylistIdleOfflineOrTesting(FalconFppdStatusDto status)
        {
            string playlistName = status.Current_PlayList.Playlist.ToLower();
            return (status == null || (playlistName == PlaylistIgnoreName.Testing || playlistName == PlaylistIgnoreName.Offline));
        }

    }
}