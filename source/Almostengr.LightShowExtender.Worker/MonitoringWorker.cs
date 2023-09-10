using Almostengr.LightShowExtender.DomainService.Monitoring;

namespace Almostengr.LightShowExtender.Worker;

public class MonitoringWorker : BackgroundService
{
    private readonly IMonitoringService _monitoringService;

    public MonitoringWorker(IMonitoringService monitoringService)
    {
        _monitoringService = monitoringService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            TimeSpan delayTime = await _monitoringService.CheckFppStatus();
            await Task.Delay(delayTime, stoppingToken);
        }
    }
}
