using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Almostengr.FalconPiMonitor.ConsoleCmd
{
    public abstract class BaseConsoleCmd
    {
        public string SystemdDirectory = "/lib/systemd/system";
        public string ServiceFilename = "falconpimonitor.service";
        public string AppDirectory =
            System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        private HttpClient _httpClient = null;

        public void ConsoleMessage(string message)
        {
            Console.WriteLine("{0} {1}", DateTime.Now.ToString(), message);
        }

        public async Task<T> GetRequestAsync<T>(string url) where T : class
        {
            string emailAddress = System.Configuration.ConfigurationManager.AppSettings.Get("EmailAddress");
            string websiteUrl = System.Configuration.ConfigurationManager.AppSettings.Get("WebsiteUrl");

            if (string.IsNullOrEmpty(emailAddress) || string.IsNullOrEmpty(websiteUrl))
            {
                Console.WriteLine(emailAddress);
                Console.WriteLine(websiteUrl);
                throw new Exception("Email address and Website URl need to be provided in configuration file");
            }

            if (_httpClient == null)
            {
                _httpClient = new HttpClient();
            }

            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd($"({websiteUrl}, {emailAddress})");

            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            }
            else
            {
                throw new System.Exception(response.ReasonPhrase);
            }
        }
    }
}