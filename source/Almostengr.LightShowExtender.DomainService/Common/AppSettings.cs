namespace Almostengr.LightShowExtender.DomainService.Common;

public class AppSettings
{
    public Monitoring Monitoring { get; init; } = new();
    public string EngineeringUrl { get; init; } = string.Empty;
    public string FalconPlayerUrl { get; init; } = string.Empty;

    public class Monitoring
    {
        public double MaxCpuTemperatureC { get; init; } = 60.0;
        public string NwsStationId {get ;init; } = "KMGM";
    }
}