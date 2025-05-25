using Almostengr.HpLightShow.Core.Countdowns.DataTransferObjects;
using Almostengr.HpLightShow.Core.Countdowns.Service;

namespace Almostengr.HpLightShow.WebApi.Workers;

internal sealed class CountdownWorker : BackgroundService
{
    private readonly ICountdownService _service;
    private readonly ILogger<CountdownWorker> _logger;

    public CountdownWorker(
        ICountdownService service,
        ILogger<CountdownWorker> logger
        )
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
            ChristmasCountdownResource resource = new(currentDate);

            var result = await _service.ExecuteAsync(resource);
            if (result.Failed)
            {
                result.Errors.ToList().ForEach(e => _logger.LogError(e));
                hoursDelay = ERROR_DELAY;
            }

            await Task.Delay(TimeSpan.FromHours(hoursDelay), stoppingToken);
        }
    }
}
