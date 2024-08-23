namespace Almostengr.FalconPiPlayer;

public sealed class FppAppSettings
{
    public string ApiUrl { get; init; } = "http://localhost";
    public double MaxCpuTemperatureC { get; init; } = 60.0;
}