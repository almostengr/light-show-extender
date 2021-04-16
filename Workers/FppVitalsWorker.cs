using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiMonitor.Models;
using Microsoft.Extensions.Logging;
using Tweetinvi;

namespace Almostengr.FalconPiTwitter.Workers
{
    public class FppVitalsWorker : BaseWorker
    {
        private readonly AppSettings _appSettings;
        private readonly ILogger<BaseWorker> _logger;
        private readonly ITwitterClient _twitterClient;

        public FppVitalsWorker(ILogger<BaseWorker> logger, AppSettings appSettings, HttpClient httpClient, ITwitterClient twitterClient) 
            : base(logger, appSettings, httpClient, twitterClient)
        {
            _appSettings = appSettings;
            _logger = logger;
            _twitterClient = twitterClient;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _httpClient.BaseAddress = new Uri(_appSettings.FalconPiPlayerUrl);
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Monitoring vitals of remote instances");

            TimeSpan delay = TimeSpan.FromMinutes(5);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(delay);
            }
        }
    }
}