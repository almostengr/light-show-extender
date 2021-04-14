using System.Threading.Tasks;
using Almostengr.FalconPiMonitor.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Almostengr.FalconPiMonitor.ServicesBase
{
    public abstract class FppBaseService : BaseService
    {
        public string masterFppInstance {get; private set;}

        public FppBaseService(ILogger<FppBaseService> logger, IConfiguration configuration) : base(logger, configuration)
        {
            // masterFppUrl = 
            //     AppSettings.FalconPiPlayers.Find(f => f.FalconPiPlayerMode.ToLower() == "master").Hostname;
            masterFppInstance = GetMasterOrStandaloneInstance();
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

        protected string GetMasterOrStandaloneInstance()
        {
            return AppSettings.FalconPiPlayers
                    .Find(p => p.FalconPiPlayerMode.ToLower() == "master" ||
                        p.FalconPiPlayerMode.ToLower() == "player")
                    .Hostname;
        }

        public async Task StopShowGracefully()
        {
            
        }

    } // end class
}