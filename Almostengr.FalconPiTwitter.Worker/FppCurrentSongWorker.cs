using Almostengr.FalconPiTwitter.Services;

namespace Almostengr.FalconPiTwitter.Worker
{
    public class FppCurrentSongWorker : BackgroundService
    {
        private readonly IFppService _fppService;

        public FppCurrentSongWorker(IFppService fppService)
        {
            _fppService = fppService;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _fppService.ExecuteCurrentSongWorkerAsync(stoppingToken);
        }

    }
}