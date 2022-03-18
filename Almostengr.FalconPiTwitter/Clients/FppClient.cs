using System;
using System.Net.Http;
using System.Threading.Tasks;
using Almostengr.FalconPiTwitter.Common.Constants;
using Almostengr.FalconPiTwitter.DataTransferObjects;
using Microsoft.Extensions.Logging;

namespace Almostengr.FalconPiTwitter.Clients
{
    public class FppClient : BaseClient, IFppClient
    {
        private readonly ILogger<FppClient> _logger;
        private readonly HttpClient _httpClient;
        
        public FppClient(ILogger<FppClient> logger) : base(logger)
        {
            _logger = logger;
            _httpClient = new HttpClient();
        }

        public async Task<FalconMediaMetaDto> GetCurrentSongMetaDataAsync(string currentSong)
        {
            _logger.LogDebug("Getting current song meta data");

            if (string.IsNullOrEmpty(currentSong))
            {
                _logger.LogWarning("No song provided");
                return new FalconMediaMetaDto();
            }

            _httpClient.BaseAddress = new Uri(AppConstants.Localhost);

            return await HttpGetAsync<FalconMediaMetaDto>(_httpClient, $"api/media/{currentSong}/meta");
        }

        public async Task<FalconFppdStatusDto> GetFppdStatusAsync(string address)
        {
            _httpClient.BaseAddress = new Uri(address);
            return await HttpGetAsync<FalconFppdStatusDto>(_httpClient, "api/fppd/status");
        }

        public async Task<FalconFppdMultiSyncSystemsDto> GetMultiSyncStatusAsync(string address)
        {
            _httpClient.BaseAddress = new Uri(address);
            return await HttpGetAsync<FalconFppdMultiSyncSystemsDto>(_httpClient, "api/fppd/multiSyncSystems");
        }
    }
}