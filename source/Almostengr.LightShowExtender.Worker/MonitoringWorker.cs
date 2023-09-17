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
        TimeSpan delayTime = TimeSpan.FromMinutes(5);
        while (!stoppingToken.IsCancellationRequested)
        {
            delayTime = await _monitoringService.UpdateSensorDataAsync();
            await Task.Delay(delayTime, stoppingToken);
        }
    }
}
