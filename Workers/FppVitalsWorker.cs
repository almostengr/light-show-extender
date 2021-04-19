using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiMonitor.Models;
using Microsoft.Extensions.Logging;
using Tweetinvi;

namespace Almostengr.FalconPiTwitter.Workers
{
    public class FppVitalsWorker : BaseWorker, IFppVitalsWorker
    {
        private readonly AppSettings _appSettings;
        private readonly ILogger<BaseWorker> _logger;
        private readonly ITwitterClient _twitterClient;
        private HttpClient _httpClient;

        public FppVitalsWorker(ILogger<BaseWorker> logger, AppSettings appSettings, ITwitterClient twitterClient)
             : base(logger, appSettings, twitterClient)
        {
            _appSettings = appSettings;
            _logger = logger;
            _twitterClient = twitterClient;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Monitoring vitals of FPP instances");
            _httpClient = new HttpClient();
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Done monitoring vitals of FPP instances");
            return base.StopAsync(cancellationToken);
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            bool tempAlarmActive = false;

            while (!stoppingToken.IsCancellationRequested)
            {
                foreach (var host in _appSettings.FalconPiPlayerUrls)
                {
                    _httpClient.BaseAddress = new Uri(host);
                    FalconFppdStatus falconFppdStatus = await GetCurrentStatusAsync(_httpClient);

                    tempAlarmActive = await IsCpuTemperatureHighAsync(falconFppdStatus.Sensors, tempAlarmActive);
                }

                await Task.Delay(TimeSpan.FromMinutes(5));
            }
        }

        private async Task<bool> IsCpuTemperatureHighAsync(IList<FalconFppdStatusSensor> sensors, bool tempAlarmActive)
        {
            foreach (var sensor in sensors)
            {
                if (sensor.Label.ToLower() == "cpu" && sensor.ValueType.ToLower() == "temperature" &&
                    sensor.Value > _appSettings.Alarm.MaxTemperature && tempAlarmActive == false)
                {
                    string alarmMessage =
                        $"Temperature warning! Threshold: {_appSettings.Alarm.MaxTemperature}, Actual: {sensor.Value}";
                    await TweetAlarmAsync(alarmMessage);
                    return true;
                }
            } // end for
            return false;
        }

        public async Task TweetAlarmAsync(string alarmMessage)
        {
            _logger.LogWarning(alarmMessage);

            alarmMessage = string.IsNullOrEmpty(_appSettings.Alarm.TwitterAlarmUser) ?
                alarmMessage :
                string.Concat(_appSettings.Alarm.TwitterAlarmUser, " ", alarmMessage);

            var result = await _twitterClient.Tweets.PublishTweetAsync(alarmMessage + DateTime.Now.ToLongTimeString());
            _logger.LogInformation("Tweet result: " + result);
        }

    }
}