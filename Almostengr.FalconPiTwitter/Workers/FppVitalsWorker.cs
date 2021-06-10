using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiTwitter.Models;
using Microsoft.Extensions.Logging;
using Tweetinvi;

namespace Almostengr.FalconPiTwitter.Workers
{
    public class FppVitalsWorker : BaseWorker, IFppVitalsWorker
    {
        private readonly AppSettings _appSettings;
        private readonly ILogger<FppVitalsWorker> _logger;
        private readonly ITwitterClient _twitterClient;
        private readonly HttpClient _httpClient;
        private int _alarmCount = 0;

        public FppVitalsWorker(ILogger<FppVitalsWorker> logger, AppSettings appSettings, ITwitterClient twitterClient)
             : base(logger, appSettings, twitterClient)
        {
            _appSettings = appSettings;
            _logger = logger;
            _twitterClient = twitterClient;

            _httpClient = new HttpClient();
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int previousHour = 0;

            while (!stoppingToken.IsCancellationRequested)
            {
                previousHour = ResetAlarmCount(previousHour);

                string host = "https://localhost";

                try
                {
                    _logger.LogInformation("Checking vitals for " + host);

                    _httpClient.BaseAddress = new Uri(host);
                    FalconFppdStatus falconFppdStatus = await GetCurrentStatusAsync(_httpClient);

                    _alarmCount += await IsCpuTemperatureHighAsync(falconFppdStatus.Sensors);
                }
                catch (NullReferenceException ex)
                {
                    _logger.LogError(string.Concat("Null Exception occurred: ", ex.Message));
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogError(string.Concat("Are you connected to internet? HttpRequest Exception occured: ", ex.Message));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, string.Concat(ex.GetType(), ex.Message));
                }

                await Task.Delay(TimeSpan.FromMinutes(5));
            }
        }

        public int ResetAlarmCount(int previousHour)
        {
            int currentHour = DateTime.Now.Hour;

            if (previousHour != currentHour && _alarmCount > 0)
            {
                _logger.LogInformation("Alarm count has been reset. " + _alarmCount + " alarms occurred in the last hour.");
                _alarmCount = 0;
            }

            return currentHour;
        }

        public async Task<int> IsCpuTemperatureHighAsync(IList<FalconFppdStatusSensor> sensors)
        {
            foreach (var sensor in sensors)
            {
                if (sensor.ValueType.ToLower() == "temperature" && sensor.Value > _appSettings.Alarm.MaxTemperature)
                {
                    string alarmMessage =
                        $"Temperature warning! Threshold: {_appSettings.Alarm.MaxTemperature}, Actual: {sensor.Value}";
                    await TweetAlarmAsync(alarmMessage);
                    return 1;
                }
            } // end for
            return 0;
        }

        public async Task TweetAlarmAsync(string alarmMessage)
        {
            _logger.LogWarning(alarmMessage);

            if (_alarmCount <= _appSettings.Alarm.MaxAlarms || _appSettings.Alarm.MaxAlarms == 0)
            {
                alarmMessage = string.IsNullOrEmpty(_appSettings.Alarm.TwitterAlarmUser) ?
                    alarmMessage :
                    string.Concat(_appSettings.Alarm.TwitterAlarmUser, " ", alarmMessage);

                var result = await _twitterClient.Tweets.PublishTweetAsync(alarmMessage + DateTime.Now.ToLongTimeString());
                _logger.LogInformation("Tweet result: " + result.CreatedAt);
            }
        }

        public override void Dispose()
        {
            _httpClient.Dispose();
            base.Dispose();
        }
    }
}