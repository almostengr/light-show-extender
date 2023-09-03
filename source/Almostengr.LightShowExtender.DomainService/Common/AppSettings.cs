namespace Almostengr.LightShowExtender.DomainService.Common;

public class AppSettings
{
    public Monitoring Monitoring { get; init; } = new();
}

public class Monitoring
{
    public List<string> AlarmUsernames { get; init; } = new();
    public int MaxAlarmsPerHour { get; init; } = 3;
    public double MaxCpuTemperatureC { get; init; } = 60.0;
    public string NwsStationId {get ;init; } = "KMGM";
}