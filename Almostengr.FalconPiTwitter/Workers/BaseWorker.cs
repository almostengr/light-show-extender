using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Almostengr.FalconPiTwitter.Workers
{
    public abstract class BaseWorker : BackgroundService
    {
        private readonly ILogger<BaseWorker> _logger;

        public BaseWorker(ILogger<BaseWorker> logger)
        {
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new System.NotImplementedException();
        }

        public async Task TaskDelayAsync(double seconds, CancellationToken stoppingToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(seconds), stoppingToken);
        }

    }
}