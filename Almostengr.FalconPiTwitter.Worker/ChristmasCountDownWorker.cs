using Almostengr.FalconPiTwitter.Common.Services;

namespace Almostengr.FalconPiTwitter.Worker
{
    public class ChristmasCountDownWorker : BackgroundService
    {
        private readonly ICountDownService _countDownService;

        public ChristmasCountDownWorker(ICountDownService countDownService)
        {
            _countDownService = countDownService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _countDownService.ExecuteChristmasCountdownAsync(stoppingToken);
        }
    }
}