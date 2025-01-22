namespace Almostengr.HpLightShow.Core.FalconPiPlayer.Common;

public sealed class FppAppSettings
{
    public string ApiUrl { get; init; } = "http://127.0.0.1";
    public double MaxCpuTemperatureC { get; init; } = 60.0;
}