using Almostengr.FalconPiTwitter.Services;

namespace Almostengr.FalconPiTwitter.Worker
{
    public class FppVitalsWorker : BackgroundService
    {
        private readonly IFppService _fppService;

        public FppVitalsWorker(IFppService fppService)
        {
            _fppService = fppService;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _fppService.ExecuteVitalsWorkerAsync(stoppingToken);
        }

    }
}