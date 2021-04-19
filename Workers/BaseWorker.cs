using System;
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
        private readonly AppSettings _appSettings;
        private readonly ITwitterClient _twitterClient;

        public BaseWorker(ILogger<BaseWorker> logger, AppSettings appSettings, ITwitterClient twitterClient)
        {
            _appSettings = appSettings;
            _twitterClient = twitterClient;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new System.NotImplementedException();
        }

        internal virtual async Task<FalconFppdStatus> GetCurrentStatusAsync(HttpClient httpClient)
        {
            return await HttpGetAsync<FalconFppdStatus>(httpClient, "api/fppd/status");
        }

        internal async Task<T> HttpGetAsync<T>(HttpClient httpClient, string route) where T : class
        {
            HttpResponseMessage response = await httpClient.GetAsync(route);

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