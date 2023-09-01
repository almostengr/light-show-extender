namespace Almostengr.LightShowExtender.DomainService.Monitoring;

public interface IMonitoringService
{
    Task<TimeSpan> UpdateCpuTemperatureAsync();

}