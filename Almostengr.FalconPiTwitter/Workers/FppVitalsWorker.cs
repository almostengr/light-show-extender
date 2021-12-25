using System;
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
    public class FppVitalsWorker : BaseWorker
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
            string previousSecondsPlayed = string.Empty;
            string previousSecondsRemaining = string.Empty;

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

                    foreach (var fppInstance in syncStatus.RemoteSystems)
                    {
                        Uri hostUri = new Uri($"http://{fppInstance.Address}");

                        _logger.LogInformation($"Checking vitals for {fppInstance.Address}");

                        _httpClient.BaseAddress = hostUri;

                        FalconFppdStatusDto falconFppdStatus = await GetFppdStatusAsync(_httpClient);

                        // check sensors
                        foreach (var sensor in falconFppdStatus.Sensors)
                        {
                            if (sensor.ValueType.ToLower() == "temperature" && sensor.Value > _appSettings.Monitoring.MaxCpuTemperatureC)
                            {
                                string alarmMessage = $"Temperature warning! Temperature: {sensor.Value}; limit: {_appSettings.Monitoring.MaxCpuTemperatureC}";
                                await PostTweetAlarmAsync(alarmMessage);
                            }
                        }

                        // check for stuck songs
                        if (falconFppdStatus.Mode_Name == FppMode.Master || falconFppdStatus.Mode_Name == FppMode.Standalone)
                        {
                            if (previousSecondsPlayed == falconFppdStatus.Seconds_Played ||
                                previousSecondsRemaining == falconFppdStatus.Seconds_Remaining)
                            {
                                await PostTweetAlarmAsync($"FPP appears to be stuck");
                            }

                            previousSecondsPlayed = falconFppdStatus.Seconds_Played;
                            previousSecondsRemaining = falconFppdStatus.Seconds_Remaining;
                        }
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

                await Task.Delay(TimeSpan.FromSeconds(DelaySeconds.Medium), stoppingToken);
            }
        }

        public override void Dispose()
        {
            _httpClient.Dispose();
            base.Dispose();
        }
    }
}