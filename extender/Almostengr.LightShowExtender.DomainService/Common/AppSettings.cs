namespace Almostengr.LightShowExtender.DomainService.Common;

public sealed class AppSettings
{
    public FalconSetting FalconPlayer { get; init; } = new();
    public uint MaxSongsBetweenPsa { get; init; } = 2;
    public uint ExtenderDelay { get; init; } = 5;
    public string StartupSequence { get; init; } = string.Empty;
    public string ShutDownSequence { get; init; } = string.Empty;
    public string ExteriorLightEntity { get; init; } = string.Empty;

    public sealed class FalconSetting
    {
        public string ApiUrl { get; init; } = "http://localhost";
        public double MaxCpuTemperatureC { get; init; } = 60.0;
    }
}