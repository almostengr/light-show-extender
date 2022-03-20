using System.Net.Http;
using System.Threading.Tasks;
using Almostengr.FalconPiTwitter.Common;
using Almostengr.FalconPiTwitter.DataTransferObjects;
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

            string hostname = GetUrlWithProtocol(_appSettings.FppHosts[0]);
            return await HttpGetAsync<FalconMediaMetaDto>(_httpClient, $"{hostname}api/media/{currentSong}/meta");
        }

        public async Task<FalconFppdStatusDto> GetFppdStatusAsync(string address)
        {
            string hostname = GetUrlWithProtocol(address);
            return await HttpGetAsync<FalconFppdStatusDto>(_httpClient, $"{hostname}api/fppd/status");
        }

        public async Task<FalconFppdMultiSyncSystemsDto> GetMultiSyncStatusAsync(string address)
        {
            string hostname = GetUrlWithProtocol(address);
            return await HttpGetAsync<FalconFppdMultiSyncSystemsDto>(_httpClient, $"{hostname}api/fppd/multiSyncSystems");
        }

        private string GetUrlWithProtocol(string address)
        {
            if (address.StartsWith("http://") == false && address.StartsWith("https://") == false)
            {
                address = "http://" + address;
            }

            if (address.EndsWith("/") == false)
            {
                address = address + "/";
            }

            return address;
        }

    }
}