namespace Almostengr.LightShowExtender.DomainService.Monitoring;

public interface IMonitoringService
{
    Task GetLatestWeatherObservationsAsync();
    Task<TimeSpan> MonitoringCheckAsync();
}