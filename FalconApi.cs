using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Almostengr.FalconPiMonitor
{
    public class FalconApi
    {
        private readonly HttpClient HttpClient = new HttpClient();
        private readonly string BaseUri = "http://falconpi/api/";

        public async Task<FalconStatus> GetCurrentStatus()
        {
            HttpResponseMessage response = await HttpClient.GetAsync(string.Concat(BaseUri, "fppd/status"));

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<FalconStatus>(response.Content.ReadAsStringAsync().Result);
            }
            else
            {
                throw new System.Exception(response.ReasonPhrase);
            }
        }

        public async Task<FalconStatusMediaMeta> GetCurrentSongMetaData(string songFileName)
        {
            HttpResponseMessage response = await HttpClient.GetAsync(string.Concat(BaseUri, "media/", songFileName, "/meta"));
            
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<FalconStatusMediaMeta>(response.Content.ReadAsStringAsync().Result);
            }
            else
            {
                throw new System.Exception(response.ReasonPhrase);
            }
        }
    }
}