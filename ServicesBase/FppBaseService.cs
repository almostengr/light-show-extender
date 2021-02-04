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
            string responseString =
                await GetRequestAsync(string.Concat(AppSettings.FalconPiPlayer.FalconUri, "fppd/status"));
            return JsonConvert.DeserializeObject<FalconFppdStatus>(responseString);
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