using Almostengr.FalconPiTwitter.Common.Services;

namespace Almostengr.FalconPiTwitter.Worker
{
    public class ChristmasCountDownWorker : BaseWorker
    {
        private readonly ICountDownService _countDownService;

        public ChristmasCountDownWorker(ICountDownService countDownService)
        {
            _countDownService = countDownService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (stoppingToken.IsCancellationRequested == false)
            {
                await _countDownService.TimeUntilChristmasAsync();
                await Task.Delay(TimeSpan.FromHours(base.GetRandomWaitTime()), stoppingToken);
            }
        }
    }
}