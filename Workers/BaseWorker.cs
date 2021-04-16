using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiMonitor.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Tweetinvi;

namespace Almostengr.FalconPiTwitter.Workers
{
    public abstract class BaseWorker : BackgroundService
    {
        internal readonly HttpClient _httpClient;
        private readonly AppSettings _appSettings;

        public BaseWorker(ILogger<BaseWorker> logger, AppSettings appSettings, HttpClient httpClient,
            ITwitterClient twitterClient)
        {
            _httpClient = httpClient;
            _appSettings = appSettings;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new System.NotImplementedException();
        }

        public async Task<T> GetAsync<T>(string route) where T : class
        {

            HttpResponseMessage response = await _httpClient.GetAsync(route);

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