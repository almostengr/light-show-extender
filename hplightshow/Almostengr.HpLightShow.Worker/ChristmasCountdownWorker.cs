using Almostengr.HpLightShow.Core.DomainHandler.ChristmasCountdown;

namespace Almostengr.HpLightShow.Worker;

public sealed class ChristmasCountdownWorker : BackgroundService
{
    private readonly ILogger<ChristmasCountdownWorker> _logger;
    private readonly ChristmasCountdownHandler _handler;

    public ChristmasCountdownWorker(
        ILogger<ChristmasCountdownWorker> logger,
        ChristmasCountdownHandler handler)
    {
        _logger = logger;
        _handler = handler;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
                DateOnly christmasDate = DateOnly.FromDateTime(new DateTime(DateTime.Now.Year, 12, 25));
                ChristmasCountdownRequest request = new(christmasDate, currentDate);

                ChristmasCountdownResult result = await _handler.ExecuteAsync(request);
                if (result.Succeeded == false)
                {
                    _logger.LogError("Error when posting about Christmas holiday countdown.");
                }

                await Task.Delay(TimeSpan.FromDays(1));
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                await Task.Delay(TimeSpan.FromHours(6));
            }
        }
    }
}
