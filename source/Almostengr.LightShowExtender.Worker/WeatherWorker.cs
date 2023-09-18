using Almostengr.LightShowExtender.DomainService.Monitoring;

namespace Almostengr.LightShowExtender.Worker;

public class WeatherWorker : BackgroundService
{
    private readonly IMonitoringService _monitoringService;

    public WeatherWorker(IMonitoringService monitoringService)
    {
        _monitoringService = monitoringService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _monitoringService.GetLatestWeatherObservationsAsync();
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}