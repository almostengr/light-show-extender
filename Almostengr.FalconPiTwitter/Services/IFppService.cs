using System;
using System.Threading.Tasks;
using Almostengr.FalconPiTwitter.DataTransferObjects;

namespace Almostengr.FalconPiTwitter.Services
{
    public interface IFppService : IBaseService
    {
        string TimeUntilNextLightShow(DateTime currentDateTime, string startTime);
        bool IsPlaylistIdleOfflineOrTesting(FalconFppdStatusDto status);
        string TimeUntilChristmas(DateTime currentDateTime);
        Task<FalconFppdMultiSyncSystemsDto> GetMultiSyncStatusAsync(string address);
        Task<FalconFppdStatusDto> GetFppdStatusAsync(string address);
        void ResetAlarmCount();
        Task<FalconMediaMetaDto> GetCurrentSongMetaDataAsync(string current_Song);
        Task CheckCpuTemperatureAsync(FalconFppdStatusDto status);
        Task CheckStuckSongAsync(FalconFppdStatusDto status, string previousSecondsPlayed, string previousSecondsRemaining);
        Task PostChristmasCountDownWhenIdleAsync(FalconFppdStatusDto status);
    }
}