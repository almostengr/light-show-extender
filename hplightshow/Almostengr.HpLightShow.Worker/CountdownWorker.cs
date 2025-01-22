using Almostengr.HpLightShow.Core.Countdowns;

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
            try
            {
                DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
                var countdownDto = new ChristmasCountdownDto(currentDate);

                var result = await _service.PostCountdownAsync(countdownDto);
                if (result.Failed)
                {
                    result.Errors.ToList().ForEach(e => _logger.LogWarning(e));
                    hoursDelay = ERROR_DELAY;
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                hoursDelay = ERROR_DELAY;
            }

            await Task.Delay(TimeSpan.FromHours(hoursDelay), stoppingToken);
        }
    }
}
