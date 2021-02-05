using System.Threading.Tasks;
using Almostengr.FalconPiMonitor.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Almostengr.FalconPiMonitor.ServicesBase
{
    public abstract class FppBaseService : BaseService
    {
        public FppBaseService(ILogger<FppBaseService> logger, IConfiguration configuration) : base(logger, configuration)
        {
        }

        public async Task<FalconFppdStatus> GetCurrentStatusAsync()
        {
            return await GetRequestAsync<FalconFppdStatus>(string.Concat(AppSettings.FalconPiPlayer.FalconUri, "fppd/status"));
        }

        public bool IsTestingOrOfflinePlaylist(string playlistName)
        {
            playlistName = playlistName.ToLower();

            if (playlistName.Contains("offline") || playlistName.Contains("test"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    } // end class
}