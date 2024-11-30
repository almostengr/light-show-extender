using Almostengr.LightShowExtender.Worker.DomainService;

namespace Almostengr.LightShowExtender.Worker;

internal sealed class HolidayCountdownWorker : BackgroundService
{
    private readonly TwitterAppSettings _twitterAppSettings;
    private readonly ILogger<HolidayCountdownWorker> _logger;

    public HolidayCountdownWorker(
        TwitterAppSettings twitterAppSettings,
        ILogger<HolidayCountdownWorker> logger)
    {
        _twitterAppSettings = twitterAppSettings;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                HolidayCountdownCommandHandler handler = new(_twitterAppSettings);
                HolidayCountdownCommand command = new();
                await handler.ExecuteAsync(stoppingToken, command);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
            }

            await Task.Delay(TimeSpan.FromHours(24));
        }
    }
}