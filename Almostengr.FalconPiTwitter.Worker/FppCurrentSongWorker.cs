using Almostengr.FalconPiTwitter.Common.Services;

namespace Almostengr.FalconPiTwitter.Worker
{
    public class FppCurrentSongWorker : BackgroundService
    {
        private readonly IFppCurrentSongService _currentSongService;

        public FppCurrentSongWorker(IFppCurrentSongService currentSongService)
        {
            _currentSongService = currentSongService;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _currentSongService.ExecuteCurrentSongWorkerAsync(stoppingToken);
        }

    }
}