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
            var delayTime = await _monitoringService.UpdateCpuTemperatureAsync();
            await Task.Delay(delayTime, stoppingToken);
        }
    }
}
