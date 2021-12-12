using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiTwitter.Constants;
using Almostengr.FalconPiTwitter.DataTransferObjects;
using Almostengr.FalconPiTwitter.Settings;
using Microsoft.Extensions.Logging;
using Tweetinvi;

namespace Almostengr.FalconPiTwitter.Workers
{
    public class FppVitalsWorker : BaseWorker, IFppVitalsWorker
    {
        private readonly AppSettings _appSettings;
        private readonly ILogger<FppVitalsWorker> _logger;
        private readonly HttpClient _httpClient;

        public FppVitalsWorker(ILogger<FppVitalsWorker> logger, AppSettings appSettings, ITwitterClient twitterClient)
             : base(logger, appSettings, twitterClient)
        {
            _appSettings = appSettings;
            _logger = logger;

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = AppConstants.Localhost;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                DateTime currentDateTime = DateTime.Now;

                if (currentDateTime.Minute > 53 && AlarmCount > 0)
                {
                    _logger.LogInformation($"Alarm count reset. Previous count: {AlarmCount}");
                    AlarmCount = 0;
                }

                try
                {
                    _httpClient.BaseAddress = AppConstants.Localhost;
                    FalconFppdMultiSyncSystemsDto syncStatus = await GetMultiSyncStatusAsync(_httpClient);

                    foreach (var fppInstance in syncStatus.Systems)
                    {
                        await CheckInstanceVitals(fppInstance);
                    }
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogError(ExceptionMessage.NoInternetConnection + ex.Message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }

                await Task.Delay(TimeSpan.FromSeconds(DelaySeconds.Long));
            }
        }

        public async Task CheckInstanceVitals(RemoteSystems fppInstance)
        {
            Uri hostUri = new Uri($"http://{fppInstance.Address}");

            _logger.LogInformation($"Checking vitals for {fppInstance.Address}");

            _httpClient.BaseAddress = hostUri;

            FalconFppdStatusDto falconFppdStatus = await GetFppdStatusAsync(_httpClient);

            await CheckAllSensors(falconFppdStatus.Sensors);
        }

        public async Task CheckAllSensors(IList<FalconFppdStatusSensor> sensors)
        {
            foreach (var sensor in sensors)
            {
                if (sensor.ValueType.ToLower() == "temperature" && sensor.Value > _appSettings.Monitor.MaxCpuTemperatureC)
                {
                    string alarmMessage = $"Temperature warning! Temperature: {sensor.Value}; limit: {_appSettings.Monitor.MaxCpuTemperatureC}";
                    await PostTweetAlarmAsync(alarmMessage);
                }
            }
        }

        public override void Dispose()
        {
            _httpClient.Dispose();
            base.Dispose();
        }
    }
}