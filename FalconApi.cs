using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Almostengr.FalconPiMonitor
{
    public class FalconApi
    {
        private readonly HttpClient _HttpClient = new HttpClient();
        private readonly string _BaseUri = "http://falconpi/api/";

        public async Task<FalconStatus> GetCurrentStatus()
        {
            var response = await _HttpClient.GetAsync(string.Concat(_BaseUri, "fppd/status"));
            return JsonConvert.DeserializeObject<FalconStatus>(response.Content.ReadAsStringAsync().Result);
        }

        public async Task<FalconStatusMediaMeta> GetCurrentSongMetaData(string songFileName)
        {
            var response = await _HttpClient.GetAsync(string.Concat(_BaseUri, "media/", songFileName, "/meta"));
            return JsonConvert.DeserializeObject<FalconStatusMediaMeta>(response.Content.ReadAsStringAsync().Result);
        }
    }
}