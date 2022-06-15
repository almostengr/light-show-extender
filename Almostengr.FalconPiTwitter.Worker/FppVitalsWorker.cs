using Almostengr.FalconPiTwitter.Common.Services;

namespace Almostengr.FalconPiTwitter.Worker
{
    public class FppVitalsWorker : BackgroundService
    {
        private readonly IFppVitalsService _fppVitalsService;
        private readonly ISystemdService _systemdService;

        public FppVitalsWorker(IFppVitalsService fppService, ISystemdService systemdService)
        {
            _fppVitalsService = fppService;
            _systemdService = systemdService;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _systemdService.CheckForSystemdServiceFile();
            return base.StartAsync(cancellationToken);
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _fppVitalsService.ExecuteVitalsWorkerAsync(stoppingToken);
        }

    }
}