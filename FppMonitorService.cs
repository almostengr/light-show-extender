using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiMonitor.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Tweetinvi;

namespace Almostengr.FalconPiMonitor
{
    public class FppMonitorService : BackgroundService
    {
        private readonly ILogger _logger;
        private HttpClient _httpClient;
        private readonly IConfiguration _config;
        private AppSettings _appSettings;
        private TwitterClient _twitterClient;
        private bool _temperatureAlarm { get; set; }

        public FppMonitorService(ILogger<FppMonitorService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _config = configuration;
            _appSettings = new AppSettings();
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            ConfigurationBinder.Bind(_config, _appSettings);

            _httpClient = new HttpClient();
            _twitterClient = new TwitterClient(
                _appSettings.TwitterSettings.ConsumerKey,
                _appSettings.TwitterSettings.ConsumerSecret,
                _appSettings.TwitterSettings.AccessToken,
                _appSettings.TwitterSettings.AccessSecret);

            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting service. Exit program by pressing Ctrl+C");

            string previousSong = "";

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (previousSong == "")
                    {
                        await GetTwitterUsername();
                    }

                    FalconFppdStatus falconStatus = await GetCurrentStatus();
                    FalconMediaMeta falconStatusMediaMeta = await GetCurrentSongMetaData(falconStatus.Current_Song);

                    if (falconStatusMediaMeta.Format.Tags.Title == "" || falconStatusMediaMeta.Format.Tags.Title == null)
                    {
                        falconStatusMediaMeta.Format.Tags.Title = falconStatus.Current_Song_NotFile;
                    }

                    previousSong = await PostCurrentSong(
                        previousSong, falconStatusMediaMeta.Format.Tags.Title,
                        falconStatusMediaMeta.Format.Tags.Artist,
                        falconStatusMediaMeta.Format.Tags.Album,
                        falconStatus.Current_PlayList.Playlist.ToLower().Contains("offline"));

                    await TemperatureCheck(falconStatus.Sensors);
                }
                catch (NullReferenceException ex)
                {
                    _logger.LogError(string.Concat("Null Exception occurred: ", ex.Message));
                }
                catch (SocketException ex)
                {
                    _logger.LogError(string.Concat("Socket Exception occurred: ", ex.Message));
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogError(string.Concat("Are you connected to internet? HttpRequest Exception occured: ", ex.Message));
                }
                catch (Exception ex)
                {
                    _logger.LogError(string.Concat("Exception occurred: ", ex.Message));
                }

                await Task.Delay(TimeSpan.FromSeconds(_appSettings.FppMonitorSettings.RefreshInterval));
            }
        } // end executeasync

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _httpClient.Dispose();
            return base.StopAsync(cancellationToken);
        }


        #region Twitter

        private async Task GetTwitterUsername()
        {
            var user = await _twitterClient.Users.GetAuthenticatedUserAsync();
            _logger.LogInformation("Connected to Twitter as {user}", user);
        }

        private async Task PostTweet(string tweetText)
        {
            if (tweetText.Length > 280)
            {
                _logger.LogWarning("Tweet is too long. Truncating. BEFORE: {tweetText}", tweetText);
                tweetText = tweetText.Substring(0, 280);
            }

#if RELEASE
                var tweet = await _twitterClient.Tweets.PublishTweetAsync(tweetText);
#endif

            _logger.LogInformation("TWEETED: {tweetText}", tweetText);
        }

        #endregion


        #region FalconPiPlayer

        public async Task<FalconFppdStatus> GetCurrentStatus()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(
                string.Concat(_appSettings.FalconPiPlayerSettings.FalconUri, "fppd/status"));

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<FalconFppdStatus>(response.Content.ReadAsStringAsync().Result);
            }
            else
            {
                throw new System.Exception(response.ReasonPhrase);
            }
        }

        public async Task<FalconMediaMeta> GetCurrentSongMetaData(string songFileName)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(
                string.Concat(_appSettings.FalconPiPlayerSettings.FalconUri, "media/", songFileName, "/meta"));

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<FalconMediaMeta>(response.Content.ReadAsStringAsync().Result);
            }
            else
            {
                throw new System.Exception(response.ReasonPhrase);
            }
        }

        #endregion


        private async Task<string> PostCurrentSong(string prevSongTitle, string currSongTitle,
            string songArtist = null, string songAlbum = null, bool showOffline = false)
        {
            if (prevSongTitle == currSongTitle)
            {
                return prevSongTitle;
            }

            if (showOffline && _appSettings.FppMonitorSettings.PostOffline == false)
            {
                _logger.LogInformation($"Show is offline. Not posting song \"{currSongTitle}\"");
                return currSongTitle;
            }

            string tweet = "Playing ";

            if (string.IsNullOrEmpty(currSongTitle) == false)
            {
                tweet = string.Concat(tweet, "\"", currSongTitle, "\"");
            }
            else
            {
                _logger.LogInformation("Not tweeting song as it does not have title.");
                return prevSongTitle;
            }

            if (string.IsNullOrEmpty(songArtist) == false)
            {
                tweet = string.Concat(tweet, " by ", songArtist);
            }

            if (string.IsNullOrEmpty(songAlbum) == false)
            {
                tweet = string.Concat(tweet, " (", songAlbum, ")");
            }

            tweet = string.Concat(tweet, " at ", DateTime.Now.ToLongTimeString());

            if (showOffline)
            {
                tweet = string.Concat(tweet, " [Offline]");
            }

            await PostTweet(tweet);

            return currSongTitle;
        }

        // private void CheckSchedulerStatus()
        // {
        // }

        // private void NextShowTime(FalconStatusCurrentPlayList currentPlayList,
        //     FalconStatusNextPlaylist nextPlaylist)
        // {
        //     int currentHour = DateTime.Now.Hour;
        //     if (currentPlayList.Playlist == "Offline" && AlertedNextShowtime == false &&
        //         (currentHour == 13 || currentHour == 16))
        //     {
        //         string tweet = string.Concat("Next showtime is ", nextPlaylist.Start_Time);
        //         AlertedNextShowtime = true;
        //     }
        //     else if (currentHour != 13 && currentHour != 16)
        //     {
        //         AlertedNextShowtime = false;
        //     }
        // }

        private async Task TemperatureCheck(IList<FalconFppdStatusSensor> sensors)
        {
            if (Double.IsNegative(_appSettings.AlarmSettings.TempThreshold) || Double.IsNaN(_appSettings.AlarmSettings.TempThreshold))
            {
                return;
            }

            foreach (var sensor in sensors)
            {
                if (sensor.ValueType.ToLower() == "temperature")
                {
                    string tempAlert = string.Concat(sensor.Value.ToString(), "C, ", sensor.DegreesCToF(), "F");
                    string preText = null;

                    if (sensor.Value >= _appSettings.AlarmSettings.TempThreshold && _temperatureAlarm == false)
                    {
                        _temperatureAlarm = true;
                        preText = "High temperature alert";
                        _logger.LogCritical(tempAlert);
                    }
                    else if (sensor.Value < _appSettings.AlarmSettings.TempThreshold && _temperatureAlarm == true)
                    {
                        _temperatureAlarm = false;
                        preText = "Temperature below threshold";
                        _logger.LogWarning(tempAlert);
                    }

                    if (string.IsNullOrEmpty(preText) == false)
                    {
                        await PostTweet(string.Concat(_appSettings.AlarmSettings.TwitterUser, " ",
                            preText, " ", tempAlert));
                    }

                    break;
                }
            }
        }
    }
}