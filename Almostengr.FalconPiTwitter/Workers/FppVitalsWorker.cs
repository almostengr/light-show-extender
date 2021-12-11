using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiTwitter.DataTransferObjects;
using Microsoft.Extensions.Logging;
using Tweetinvi;

namespace Almostengr.FalconPiTwitter.Workers
{
    public class FppVitalsWorker : BaseWorker, IFppVitalsWorker
    {
        private readonly AppSettings _appSettings;
        private readonly ILogger<FppVitalsWorker> _logger;
        private readonly HttpClient _httpClient;
        private int _alarmCount = 0;

        public FppVitalsWorker(ILogger<FppVitalsWorker> logger, AppSettings appSettings, ITwitterClient twitterClient)
             : base(logger, appSettings, twitterClient)
        {
            _appSettings = appSettings;
            _logger = logger;

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = HostUri;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int previousHour = 0;

            while (!stoppingToken.IsCancellationRequested)
            {
                previousHour = ResetAlarmCount(previousHour);

                try
                {
                    _logger.LogInformation("Checking vitals for " + HostUri);

                    FalconFppdStatusDto falconFppdStatus = await GetCurrentStatusAsync(_httpClient);

                    _alarmCount += await IsCpuTemperatureHighAsync(falconFppdStatus.Sensors);
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogError("Are you connected to internet? Is FFPPd running? " + ex.Message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
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
            const double maxCpuTemperature = 60.0;

            foreach (var sensor in sensors)
            {
                if (sensor.ValueType.ToLower() == "temperature" && sensor.Value > maxCpuTemperature)
                {
                    string alarmMessage = $"Temperature warning! Temperature: {sensor.Value}";
                    await TweetAlarmAsync(alarmMessage);
                    return 1;
                }
            } // end for
            return 0;
        }

        public async Task TweetAlarmAsync(string alarmMessage)
        {
            _logger.LogWarning(alarmMessage);

            const int maxAllowedAlarms = 3;

            if (_alarmCount <= maxAllowedAlarms)
            {
                alarmMessage = _appSettings.Twitter.AlarmUsers.Count > 0 ?
                    alarmMessage :
                    string.Concat(_appSettings.Twitter.AlarmUsers, " ", alarmMessage);

                await PostTweetAsync(alarmMessage + " " + DateTime.Now.ToLongTimeString());
            }
        }

        public override void Dispose()
        {
            _httpClient.Dispose();
            base.Dispose();
        }
    }
}