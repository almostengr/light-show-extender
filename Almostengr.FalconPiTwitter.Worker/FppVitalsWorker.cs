using Almostengr.FalconPiTwitter.Common.Services;

namespace Almostengr.FalconPiTwitter.Worker
{
    public class FppVitalsWorker : BackgroundService
    {
        private readonly IFppService _fppService;
        private readonly ISystemdService _systemdService;

        public FppVitalsWorker(IFppService fppService, ISystemdService systemdService)
        {
            _fppService = fppService;
            _systemdService = systemdService;
        }

        public override Task ExecuteTask => base.ExecuteTask;

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _systemdService.CheckForSystemdServiceFile();
            return base.StartAsync(cancellationToken);
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _fppService.ExecuteVitalsWorkerAsync(stoppingToken);
        }

    }
}