using Almostengr.FalconPiTwitter.Clients;
using Almostengr.FalconPiTwitter.Common.Constants;
using Almostengr.FalconPiTwitter.DataTransferObjects;
using Microsoft.Extensions.Logging;
using Tweetinvi.Exceptions;

namespace Almostengr.FalconPiTwitter.Common.Services
{
    public class FppVitalsService : IFppVitalsService
    {
        private readonly IFppClient _fppClient;
        private readonly AppSettings _appSettings;
        private readonly ITwitterService _twitterService;
        private readonly ILogger<FppVitalsService> _logger;
        private int AlarmCount = 0;

        public FppVitalsService(ILogger<FppVitalsService> logger, AppSettings appSettings,
            ITwitterService twitterService, IFppClient fppClient)
        {
            _fppClient = fppClient;
            _appSettings = appSettings;
            _twitterService = twitterService;
            _logger = logger;
        }

        private void ResetAlarmCount()
        {
            TimeSpan currentTime = DateTime.Now.TimeOfDay;

            if (AlarmCount > 0 && currentTime.Minutes >= 55)
            {
                _logger.LogWarning($"Alarm count reset. Previous count: {AlarmCount}");
                AlarmCount = 0;
            }
        }

        private async Task CheckCpuTemperatureAsync(FalconFppdStatusDto status)
        {
            if (status == null)
            {
                return;
            }

            foreach (var sensor in status.Sensors)
            {
                string alarmMessage = string.Empty;
                if (sensor.Value >= _appSettings.Monitoring.MaxCpuTemperatureC &&
                    sensor.ValueType.ToLower() == SensorValueType.Temperature)
                {
                    alarmMessage = $"Temperature warning! Temperature: {sensor.Value}; limit: {_appSettings.Monitoring.MaxCpuTemperatureC}";
                }

                if (sensor.ValueType.ToLower() == SensorValueType.Temperature)
                {
                    _logger.LogInformation($"Temperature {sensor.Value}");
                }

                if (string.IsNullOrEmpty(alarmMessage) == false)
                {
                    await _twitterService.PostTweetAlarmAsync(alarmMessage);
                }
            } // end foreach
        }

        private async Task CheckStuckSongAsync(FalconFppdStatusDto status, string previousSecondsPlayed, string previousSecondsRemaining)
        {
            if (status.Mode_Name == FppMode.Remote || string.IsNullOrEmpty(status.Current_Song))
            {
                return;
            }

            if (previousSecondsPlayed == status.Seconds_Played ||
                previousSecondsRemaining == status.Seconds_Remaining)
            {
                await _twitterService.PostTweetAlarmAsync(ExceptionMessage.FppFrozen);
            }
        }

        public async Task ExecuteVitalsWorkerAsync(CancellationToken stoppingToken)
        {
            string previousSecondsPlayed = string.Empty;
            string previousSecondsRemaining = string.Empty;

            while (stoppingToken.IsCancellationRequested == false)
            {
                ResetAlarmCount();

                FalconFppdMultiSyncSystemsDto syncStatus = null;

                try
                {
                    syncStatus = await _fppClient.GetMultiSyncStatusAsync(_appSettings.FppHosts[0]);

                    foreach (var fppInstance in syncStatus.Systems)
                    {
                        _logger.LogInformation($"Checking vitals for {fppInstance.Hostname} ({fppInstance.Address})");

                        FalconFppdStatusDto falconFppdStatus = await _fppClient.GetFppdStatusAsync(fppInstance.Address);

                        if (falconFppdStatus == null)
                        {
                            _logger.LogError(ExceptionMessage.FppOffline);
                            continue;
                        }

                        await CheckCpuTemperatureAsync(falconFppdStatus);

                        await CheckStuckSongAsync(falconFppdStatus, previousSecondsPlayed, previousSecondsRemaining);

                        if (falconFppdStatus.Mode_Name == FppMode.Master || falconFppdStatus.Mode_Name == FppMode.Standalone)
                        {
                            previousSecondsPlayed = falconFppdStatus.Seconds_Played;
                            previousSecondsRemaining = falconFppdStatus.Seconds_Remaining;
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogError(ex, ExceptionMessage.NoInternetConnection + ex.Message);
                }
                catch (TwitterException ex)
                {
                    _logger.LogError(ex, ex.Message);
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }

                await Task.Delay(TimeSpan.FromSeconds(DelaySeconds.Long), stoppingToken);
            }
        }
    }
}