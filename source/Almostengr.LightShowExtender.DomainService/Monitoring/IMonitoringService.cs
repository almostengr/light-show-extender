namespace Almostengr.LightShowExtender.DomainService.Monitoring;

public interface IMonitoringService
{
    Task LatestWeatherObservationAsync();
    Task<TimeSpan> CheckFppStatus();
}