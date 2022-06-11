using Almostengr.FalconPiTwitter.Common.Services;

namespace Almostengr.FalconPiTwitter.Worker
{
    public class LightShowCountdownWorker : BaseWorker
    {
        private readonly ICountDownService _countDownService;

        public LightShowCountdownWorker(ICountDownService countDownService)
        {
            _countDownService = countDownService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (stoppingToken.IsCancellationRequested == false)
            {
                await _countDownService.TimeUntilNextLightShowAsync();
                await Task.Delay(TimeSpan.FromHours(base.GetRandomWaitTime()), stoppingToken);
            }
        }

    }
}