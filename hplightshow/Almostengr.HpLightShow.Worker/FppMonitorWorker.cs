using Almostengr.HpLightShow.Core.DomainHandler.FppMonitor;

namespace Almostengr.HpLightShow.Worker;

internal sealed class FppMonitorWorker : BackgroundService
{
    private readonly ILogger<FppMonitorWorker> _logger;
    private readonly FppMonitorHandler _fppMonitorHandler;

    public FppMonitorWorker(
        ILogger<FppMonitorWorker> logger,
        FppMonitorHandler fppMonitorHandler)
    {
        _logger = logger;
        _fppMonitorHandler = fppMonitorHandler;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await _fppMonitorHandler.ExecuteAsync();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
            }

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}