using System.Threading;
using System.Threading.Tasks;
using Almostengr.FalconPiTwitter.Settings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Almostengr.FalconPiTwitter.Workers
{
    public abstract class BaseWorker : BackgroundService
    {
        private readonly ILogger<BaseWorker> _logger;

        public BaseWorker(ILogger<BaseWorker> logger, AppSettings appSettings)
        {
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new System.NotImplementedException();
        }

    }
}