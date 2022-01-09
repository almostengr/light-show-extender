using System.Net.Http;
using System.Threading.Tasks;
using Almostengr.FalconPiTwitter.DataTransferObjects;
using Almostengr.FalconPiTwitter.Settings;
using Microsoft.Extensions.Logging;

namespace Almostengr.FalconPiTwitter.Clients
{
    public class FppClient : BaseClient, IFppClient
    {
        private readonly ILogger<FppClient> _logger;
        private readonly HttpClient _httpClient;
        private readonly AppSettings _appSettings;

        public FppClient(ILogger<FppClient> logger, AppSettings appSettings) : base(logger)
        {
            _logger = logger;
            _httpClient = new HttpClient();
            _appSettings = appSettings;
        }

        public async Task<FalconMediaMetaDto> GetCurrentSongMetaDataAsync(string currentSong)
        {
            _logger.LogDebug("Getting current song meta data");

            if (string.IsNullOrEmpty(currentSong))
            {
                _logger.LogWarning("No song provided");
                return new FalconMediaMetaDto();
            }

            string route = AssignRoute($"{_appSettings.FppHosts[0]}/api/media/{currentSong}/meta");
            return await HttpGetAsync<FalconMediaMetaDto>(_httpClient, route);
        }

        public async Task<FalconFppdStatusDto> GetFppdStatusAsync(string address)
        {
            string route = AssignRoute($"{address}/api/fppd/status");
            return await HttpGetAsync<FalconFppdStatusDto>(_httpClient, route);
        }

        public async Task<FalconFppdMultiSyncSystemsDto> GetMultiSyncStatusAsync(string address)
        {
            string route = AssignRoute($"{address}/api/fppd/status");
            return await HttpGetAsync<FalconFppdMultiSyncSystemsDto>(_httpClient, route);
        }
    }
}