using Almostengr.FalconPiTwitter.Clients;
using Almostengr.FalconPiTwitter.Common.Constants;
using Almostengr.FalconPiTwitter.Common.Extensions;
using Almostengr.FalconPiTwitter.Common.DataTransferObjects;
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
            if (status.IsNull())
            {
                return;
            }

            foreach (var sensor in status.Sensors)
            {
                string alarmMessage = string.Empty;

                if (sensor.ValueType.ToLower() == SensorValueType.Temperature)
                {
                    double fahrenheit = ConvertCelsiusToFahrenheit(sensor.Value);
                    double limitF = ConvertCelsiusToFahrenheit(_appSettings.Monitoring.MaxCpuTemperatureC);

                    _logger.LogInformation($"Temperature {sensor.Value}C, {fahrenheit}F");

                    if (sensor.Value >= _appSettings.Monitoring.MaxCpuTemperatureC)
                    {
                        alarmMessage = $"Temperature warning! Temperature: {sensor.Value}C, {fahrenheit}F; limit: {_appSettings.Monitoring.MaxCpuTemperatureC}C, {limitF}F";
                    }
                }

                await _twitterService.PostTweetAlarmAsync(alarmMessage);
            } // end foreach
        }

        private double ConvertCelsiusToFahrenheit(double celsius)
        {
            return (celsius * 1.8) + 32;
        }

        private async Task CheckStuckSongAsync(FalconFppdStatusDto status, string previousSecondsPlayed, string previousSecondsRemaining)
        {
            if (status.Mode_Name.IsRemoteInstance() || status.Current_Song.IsNull())
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

                try
                {
                    FalconFppdMultiSyncSystemsDto syncStatus =
                        await _fppClient.GetMultiSyncStatusAsync(_appSettings.FppHosts[0]);

                    foreach (var fppInstance in syncStatus.Systems)
                    {
                        _logger.LogInformation($"Checking vitals for {fppInstance.Hostname} ({fppInstance.Address})");

                        FalconFppdStatusDto falconFppdStatus = await _fppClient.GetFppdStatusAsync(fppInstance.Address);

                        if (falconFppdStatus.IsNull())
                        {
                            _logger.LogError(ExceptionMessage.FppOffline);
                            continue;
                        }

                        await CheckCpuTemperatureAsync(falconFppdStatus);

                        await CheckStuckSongAsync(falconFppdStatus, previousSecondsPlayed, previousSecondsRemaining);

                        if (falconFppdStatus.Mode_Name.IsRemoteInstance() == false)
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