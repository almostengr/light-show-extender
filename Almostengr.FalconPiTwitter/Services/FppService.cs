using System;
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

        public FppService(ILogger<FppService> logger, AppSettings appSettings, IFppClient fppClient) : base(logger)
        {
            _fppClient = fppClient;
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
            return await _fppClient.GetFppdStatusAsync(address);
        }

        public async Task<FalconFppdMultiSyncSystemsDto> GetMultiSyncStatusAsync(string address)
        {
            return await _fppClient.GetMultiSyncStatusAsync(address);
        }

        public async Task<FalconMediaMetaDto> GetCurrentSongMetaDataAsync(string current_Song)
        {
            return await _fppClient.GetCurrentSongMetaDataAsync(current_Song);
        }
    }
}