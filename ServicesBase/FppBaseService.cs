using System.Threading.Tasks;
using Almostengr.FalconPiMonitor.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Almostengr.FalconPiMonitor.ServicesBase
{
    public abstract class FppBaseService : BaseService
    {
        public string masterFppUrl;

        public FppBaseService(ILogger<FppBaseService> logger, IConfiguration configuration) : base(logger, configuration)
        {
            masterFppUrl = 
                AppSettings.FalconPiPlayers.Find(f => f.FalconPiPlayerMode.ToLower() == "master").Hostname;
        }

        protected async Task<FalconFppdStatus> GetCurrentStatusAsync(string fppHostname)
        {
            // string responseString =
            //     await GetRequestAsync(string.Concat(AppSettings.FalconPiPlayer.FalconUri, "fppd/status"));
            // return JsonConvert.DeserializeObject<FalconFppdStatus>(responseString);
            return await GetRequestAsync<FalconFppdStatus>(string.Concat(fppHostname, "fppd/status"));
        }

        protected bool IsTestingOrOfflinePlaylist(string playlistName)
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