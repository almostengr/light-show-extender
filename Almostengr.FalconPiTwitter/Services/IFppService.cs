using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Almostengr.FalconPiTwitter.DataTransferObjects;

namespace Almostengr.FalconPiTwitter.Services
{
    public interface IFppService : IBaseService
    {
        string TimeUntilNextLightShow(DateTime currentDateTime, string startTime);
        bool IsPlaylistIdleOfflineOrTesting(FalconFppdStatusDto status);
        double GetRandomWaitTime();
        string TimeUntilChristmas(DateTime currentDateTime);
        Task<FalconFppdMultiSyncSystemsDto> GetMultiSyncStatusAsync(string address);
        Task<FalconFppdStatusDto> GetFppdStatusAsync(string address);
        Task<FalconMediaMetaDto> GetCurrentSongMetaDataAsync(string current_Song);
        Task<List<string>> GetFppHostsAsync();
        string CheckSensorData(FalconFppdStatusSensor sensor);
        bool CheckForStuckFpp(string previousSecondsPlayed, string previousSecondsRemaining, FalconFppdStatusDto falconFppdStatus);
    }
}