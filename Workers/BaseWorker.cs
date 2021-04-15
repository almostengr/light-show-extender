using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Tweetinvi;

namespace Almostengr.FalconPiTwitter.Workers
{
    public abstract class BaseWorker : BackgroundService
    {
        private readonly ILogger<BaseWorker> _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly ITwitterClient _twitterClient;

        public BaseWorker(ILogger<BaseWorker> logger, IConfiguration configuration, HttpClient httpClient,
            ITwitterClient twitterClient)
        {
            _logger = logger;
            _configuration = configuration;
            _httpClient = httpClient;
            _twitterClient = twitterClient;
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