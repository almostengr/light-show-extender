namespace Almostengr.LightShowExtender.DomainService.Common;

public sealed class AppSettings
{
    public FalconSetting FalconPlayer { get; init; } = new();
    public uint MaxSongsBetweenPsa { get; init; } = 2;
    public uint ExtenderDelay { get; init; } = 5;

    public sealed class FalconSetting
    {
        public string ApiUrl { get; init; } = "http://localhost";
        public double MaxCpuTemperatureC { get; init; } = 60.0;
    }

    public sealed class LightShow
    {
        public string ApiUrl {get; init; } = string.Empty;
        public string ApiKey {get; init; } = string.Empty;
    }
}