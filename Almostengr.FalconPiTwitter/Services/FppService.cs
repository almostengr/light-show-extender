using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Almostengr.FalconPiTwitter.Clients;
using Almostengr.FalconPiTwitter.Constants;
using Almostengr.FalconPiTwitter.DataTransferObjects;
using Almostengr.FalconPiTwitter.Settings;
using Microsoft.Extensions.Logging;

namespace Almostengr.FalconPiTwitter.Services
{
    public class FppService : BaseService, IFppService
    {
        private readonly IFppClient _fppClient;
        private readonly ILogger<FppService> _logger;
        private readonly AppSettings _appSettings;

        public FppService(ILogger<FppService> logger, AppSettings appSettings, IFppClient fppClient) : base(logger)
        {
            _fppClient = fppClient;
            _logger = logger;
            _appSettings = appSettings;
        }

        public bool IsPlaylistIdleOfflineOrTesting(FalconFppdStatusDto status)
        {
            string playlistName = status.Current_PlayList.Playlist.ToLower();
            return (status == null ||
                (playlistName == string.Empty || playlistName.Contains(PlaylistIgnoreName.Testing) || playlistName.Contains(PlaylistIgnoreName.Offline)));
        }

        public string TimeUntilNextLightShow(DateTime currentDateTime, string startTime)
        {
            string nextPlaylistDateTime = startTime.Substring(0, startTime.IndexOf(" - "));
            nextPlaylistDateTime = nextPlaylistDateTime.Replace(" @ ", "T");

            DateTime showStartDateTime = DateTime.Parse(nextPlaylistDateTime);

            string dayDiff = CalculateTimeBetween(currentDateTime, showStartDateTime);
            return $"{dayDiff} until the next Light Show. ";
        }

        public string TimeUntilChristmas(DateTime currentDateTime)
        {
            DateTime christmasDate = new DateTime(currentDateTime.Year, 12, 25, 00, 00, 00);

            if (currentDateTime < christmasDate)
            {
                string dayDiff = CalculateTimeBetween(currentDateTime, christmasDate);
                return $"{dayDiff} until Christmas. ";
            }
            else if (currentDateTime >= christmasDate && currentDateTime < christmasDate.AddDays(1))
            {
                return "Today is Christmas! ";
            }

            return string.Empty;
        }

        public double GetRandomWaitTime()
        {
            double waitHours = 7 * Random.NextDouble();

            if (waitHours < 0.5)
            {
                waitHours = 1;
            }

            _logger.LogDebug($"Waiting {waitHours} hours");
            return waitHours;
        }

        public async Task<FalconFppdStatusDto> GetFppdStatusAsync(string address)
        {
            try
            {
                return await _fppClient.GetFppdStatusAsync(address);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                _logger.LogError(ExceptionMessage.FppOffline);
                return null;
            }
        }

        public async Task<FalconFppdMultiSyncSystemsDto> GetMultiSyncStatusAsync(string address)
        {
            try
            {
                return await _fppClient.GetMultiSyncStatusAsync(address);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                _logger.LogError(ExceptionMessage.FppOffline);
                return null;
            }
        }

        public async Task<FalconMediaMetaDto> GetCurrentSongMetaDataAsync(string current_Song)
        {
            try
            {
                return await _fppClient.GetCurrentSongMetaDataAsync(current_Song);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                _logger.LogError(ExceptionMessage.FppOffline);
                return null;
            }
        }

        public async Task<List<string>> GetFppHostsAsync()
        {
            if (_appSettings.FppHosts == null)
            {
                _appSettings.FppHosts = new List<string>();
                _appSettings.FppHosts.Add(AppConstants.Localhost);

                try
                {
                    var additionalHosts = await GetMultiSyncStatusAsync(AppConstants.Localhost);

                    foreach (var host in additionalHosts.RemoteSystems)
                    {
                        _appSettings.FppHosts.Add(host.Address);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error checking for additional FPP instancess");
                }
            }

            return _appSettings.FppHosts;
        }

        public string CheckSensorData(FalconFppdStatusSensor sensor)
        {
            string alarmMessage = string.Empty;

            if (sensor.ValueType.ToLower() == "temperature" && sensor.Value > _appSettings.Monitoring.MaxCpuTemperatureC)
            {
                alarmMessage += $"Temperature warning! Temperature: {sensor.Value}; limit: {_appSettings.Monitoring.MaxCpuTemperatureC}";
            }

            return alarmMessage;
        }

        public bool CheckForStuckFpp(string previousSecondsPlayed, string previousSecondsRemaining, FalconFppdStatusDto falconFppdStatus)
        {
            if (falconFppdStatus.Mode_Name == FppMode.Master || falconFppdStatus.Mode_Name == FppMode.Standalone)
            {
                if ((previousSecondsPlayed == falconFppdStatus.Seconds_Played ||
                    previousSecondsRemaining == falconFppdStatus.Seconds_Remaining) &&
                    string.IsNullOrEmpty(falconFppdStatus.Current_PlayList.Playlist) == false)
                {
                    return true;
                }
            }

            // TODO check remote instances

            return false;
        }

    }
}