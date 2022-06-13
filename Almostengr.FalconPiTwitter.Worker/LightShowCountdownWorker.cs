using Almostengr.FalconPiTwitter.Common.Services;

namespace Almostengr.FalconPiTwitter.Worker
{
    public class LightShowCountdownWorker : BackgroundService
    {
        private readonly ICountDownService _countDownService;

        public LightShowCountdownWorker(ICountDownService countDownService)
        {
            _countDownService = countDownService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _countDownService.ExecuteLightShowCountdownAsync(stoppingToken);
        }

    }
}