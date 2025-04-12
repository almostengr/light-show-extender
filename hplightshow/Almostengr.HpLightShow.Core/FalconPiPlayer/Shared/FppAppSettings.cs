namespace Almostengr.HpLightShow.Core.FalconPiPlayer.Shared;

public sealed class FppAppSettings
{
    public DateOnly EasterStartDate { get; init; } = new(DateTime.Now.Year, 3, 26);
    public DateOnly EasterEndDate { get; init; } = new(DateTime.Now.Year, 3, 28);
    public DateOnly FatTuesdayDate { get; init; } = new(DateTime.Now.Year, 2, 28);
    public string FppApiUrl { get; init; } = "http://10.10.50.101";
    public double MaxCpuTemperatureC { get; init; } = 60.0;
    public string? SequenceOverride { get; init; } = null;
}