using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Almostengr.FalconPiMonitor
{
    public class FalconApi
    {
        private readonly HttpClient _HttpClient = new HttpClient();
        private readonly string _BaseUri = "http://falconpi/api/";
        private FalconStatus fppdStatus ;

        public async Task<string> GetCurrentSong()
        {
            var response = await _HttpClient.GetAsync(string.Concat(_BaseUri, "fppd/status"));
            fppdStatus = JsonConvert.DeserializeObject<FalconStatus>(response.Content.ReadAsStringAsync().Result);

            if (response.IsSuccessStatusCode)
                return fppdStatus.Current_Song;

            return "";
        }

        public async Task StartPlaylist()
        {
            // /api/playlist/:PlaylistName/start
            // GET	Start the playlist named :PlaylistName.
        }

        public async Task StopPlaylistAfterLoop()
        {
            // /api/playlists/stopgracefullyafterloop
            // GET	Gracefully stop the currently running playlist after completion of the current loop
        }
    }
}