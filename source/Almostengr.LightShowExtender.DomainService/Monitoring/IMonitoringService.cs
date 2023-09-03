namespace Almostengr.LightShowExtender.DomainService.Monitoring;

public interface IMonitoringService
{
    Task<TimeSpan> UpdateCpuTemperatureAsync();
    Task<DateTime> LatestWeatherObservationAsync(DateTime lastWeatherCheck);
}