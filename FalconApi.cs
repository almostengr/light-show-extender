using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Almostengr.FalconPiMonitor
{
    public class FalconApi
    {
        private readonly HttpClient HttpClient = new HttpClient();

        private string _baseUri;
        private string BaseUri
        {
            get { return _baseUri; }
            set { _baseUri = SetBaseUri(value); }
        }

        public FalconApi(string falconUrl)
        {
            BaseUri = falconUrl;
        }

        private string SetBaseUri(string uri)
        {
            uri = uri.ToLower().Replace("api/", "").Replace("api", "");

            if (uri.StartsWith("http://") == false && uri.StartsWith("https://") == false)
            {
                uri = string.Concat("http://", uri);
            }

            uri = string.Concat(uri, "/api/");
            uri = uri.Replace("//api/", "/api/");

            return uri;
        }

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