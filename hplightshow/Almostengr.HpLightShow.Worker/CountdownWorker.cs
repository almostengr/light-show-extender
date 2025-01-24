using Almostengr.HpLightShow.Core.Countdowns.DataTransferObjects;
using Almostengr.HpLightShow.Core.Countdowns.Service;

namespace Almostengr.HpLightShow.Worker;

internal sealed class CountdownWorker : BackgroundService
{
    private readonly ILogger<CountdownWorker> _logger;
    private readonly ICountdownService _service;

    public CountdownWorker(
        ILogger<CountdownWorker> logger,
        ICountdownService service)
    {
        _logger = logger;
        _service = service;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        const int ERROR_DELAY = 6;

        while (!stoppingToken.IsCancellationRequested)
        {
            int hoursDelay = 24;
            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
            ChristmasCountdownDto countdownDto = new(currentDate);

            var result = await _service.PostCountdownAsync(countdownDto);
            if (result.Failed)
            {
                result.Errors.ToList().ForEach(e => _logger.LogError(e));
                hoursDelay = ERROR_DELAY;
            }

            await Task.Delay(TimeSpan.FromHours(hoursDelay), stoppingToken);
        }
    }
}
