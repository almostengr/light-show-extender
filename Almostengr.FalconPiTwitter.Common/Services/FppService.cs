using Almostengr.FalconPiTwitter.Clients;
using Almostengr.FalconPiTwitter.Common;
using Almostengr.FalconPiTwitter.Common.Constants;
using Almostengr.FalconPiTwitter.Common.Extensions;
using Almostengr.FalconPiTwitter.DataTransferObjects;
using Microsoft.Extensions.Logging;
using Tweetinvi.Exceptions;

namespace Almostengr.FalconPiTwitter.Services
{
    public class FppService : IFppService
    {
        private readonly IFppClient _fppClient;
        private readonly AppSettings _appSettings;
        private readonly ITwitterService _twitterService;
        private readonly ILogger<FppService> _logger;
        private int AlarmCount = 0;

        public FppService(ILogger<FppService> logger, AppSettings appSettings,
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
                            break;
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

        public async Task ExecuteCurrentSongWorkerAsync(CancellationToken stoppingToken)
        {
            string previousSong = string.Empty;

            while (stoppingToken.IsCancellationRequested == false)
            {
                await Task.Delay(TimeSpan.FromSeconds(DelaySeconds.Short), stoppingToken);

                try
                {
                    FalconFppdStatusDto fppStatus = await _fppClient.GetFppdStatusAsync(_appSettings.FppHosts[0]);

                    if (fppStatus.Mode_Name == FppMode.Remote)
                    {
                        _logger.LogWarning("This is remote instance of FPP. Exiting");
                        break;
                    }

                    if (string.IsNullOrEmpty(fppStatus.Current_Song) || previousSong == fppStatus.Current_Song)
                    {
                        continue;
                    }
                    
                    FalconMediaMetaDto falconMediaMeta = await _fppClient.GetCurrentSongMetaDataAsync(fppStatus.Current_Song);

                    string songTitle =
                        string.IsNullOrEmpty(falconMediaMeta.Format.Tags.Title) ?
                        fppStatus.Current_Song.SongNameFromFileName() :
                        falconMediaMeta.Format.Tags.Title;

                    await _twitterService.PostCurrentSongAsync(
                        songTitle,
                        falconMediaMeta.Format.Tags.Artist,
                        fppStatus.Current_PlayList.Playlist);

                    previousSong = fppStatus.Current_Song;
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogError(ex.InnerException.ToString(), ExceptionMessage.NoInternetConnection + ex.Message);
                }
                catch (TwitterException ex)
                {
                    _logger.LogError(ex.InnerException.ToString(), ex.Message);
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.InnerException.ToString(), ex.Message);
                }
            }
        }

    }
}