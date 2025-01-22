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
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
                var dto = new ChristmasCountdownDto(currentDate);

                await _service.PostCountdownAsync(dto);
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
            }
        }
    }
}
