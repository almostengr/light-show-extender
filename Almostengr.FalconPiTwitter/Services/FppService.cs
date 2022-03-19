using System;
using System.Threading.Tasks;
using Almostengr.FalconPiTwitter.Clients;
using Almostengr.FalconPiTwitter.Common;
using Almostengr.FalconPiTwitter.Common.Constants;
using Almostengr.FalconPiTwitter.DataTransferObjects;

using Microsoft.Extensions.Logging;

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

        public bool IsPlaylistIdleOfflineOrTesting(FalconFppdStatusDto status)
        {
            string playlistName = status.Current_PlayList.Playlist.ToLower();
            return (status == null || (playlistName.Contains(PlaylistIgnoreName.Testing) || playlistName.Contains(PlaylistIgnoreName.Offline)));
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
            Random random = new();
            double waitHours = 0;

            while (waitHours < 0.75)
            {
                waitHours = 7 * random.NextDouble();
            }

            return waitHours;
        }
        
        public string CalculateTimeBetween(DateTime startDate, DateTime endDate)
        {
            TimeSpan timeDiff = endDate - startDate;

            string output = string.Empty;
            output += (timeDiff.Days > 0 ? (timeDiff.Days + (timeDiff.Days == 1 ? " day " : " days ")) : string.Empty);
            output += (timeDiff.Hours > 0 ? (timeDiff.Hours + (timeDiff.Hours == 1 ? " hour " : " hours ")) : string.Empty);
            output += (timeDiff.Minutes > 0 ? (timeDiff.Minutes + (timeDiff.Minutes == 1 ? " minute " : " minutes ")) : string.Empty);

            return output;
        }

        public async Task<FalconFppdStatusDto> GetFppdStatusAsync(string address)
        {
            return await _fppClient.GetFppdStatusAsync(address);
        }

        public void ResetAlarmCount()
        {
            TimeSpan currentTime = DateTime.Now.TimeOfDay;
            
            if (AlarmCount > 0 && currentTime.Minutes >= 55)
            {
                _logger.LogWarning($"Alarm count reset. Previous count: {AlarmCount}");
                AlarmCount = 0;
            }
        }

        public async Task<FalconFppdMultiSyncSystemsDto> GetMultiSyncStatusAsync(string address)
        {
            return await _fppClient.GetMultiSyncStatusAsync(address);
        }

        public async Task<FalconMediaMetaDto> GetCurrentSongMetaDataAsync(string current_Song)
        {
            return await _fppClient.GetCurrentSongMetaDataAsync(current_Song);
        }

        public async Task CheckCpuTemperatureAsync(FalconFppdStatusDto status)
        {
            foreach (var sensor in status.Sensors)
            {
                if (sensor.ValueType.ToLower() == "temperature")
                {
                    if (sensor.Value >= _appSettings.Monitoring.MaxCpuTemperatureC)
                    {
                        string alarmMessage = $"Temperature warning! Temperature: {sensor.Value}; limit: {_appSettings.Monitoring.MaxCpuTemperatureC}";
                        await _twitterService.PostTweetAlarmAsync(alarmMessage);
                    }
                    break;
                }
            }
        }
        public async Task CheckStuckSongAsync(FalconFppdStatusDto status, string previousSecondsPlayed, string previousSecondsRemaining)
        {
            if (status.Mode_Name == FppMode.Master || status.Mode_Name == FppMode.Standalone)
            {
                if (previousSecondsPlayed == status.Seconds_Played ||
                    previousSecondsRemaining == status.Seconds_Remaining)
                {
                    await _twitterService.PostTweetAlarmAsync(ExceptionMessage.FppFrozen);
                }
            }
        }

        public async Task PostChristmasCountDownWhenIdleAsync(FalconFppdStatusDto status)
        {
            if (IsPlaylistIdleOfflineOrTesting(status))
            {
                string tweetString = TimeUntilChristmas(DateTime.Now);
                tweetString += _twitterService.GetRandomChristmasHashTag();

                await _twitterService.PostTweetAsync(tweetString);
            }
        }

    }
}