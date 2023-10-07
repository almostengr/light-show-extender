namespace Almostengr.LightShowExtender.DomainService.Common;

public sealed class AppSettings
{
    public FrontEndSetting FrontEnd { get; init; } = new();
    public FalconSetting FalconPlayer { get; init; } = new();
    public string NwsApiUrl { get; init; } = string.Empty;
    public string NwsStationId { get; init; } = "KMGM";

    public sealed class FalconSetting
    {
        public string ApiUrl { get; init; } = "http://localhost";
        public double MaxCpuTemperatureC { get; init; } = 60.0;
    }

    public sealed class FrontEndSetting
    {
        public string ApiUrl { get; init; } = string.Empty;
        public string ApiKey { get; init; } = string.Empty;
    }
}