namespace Almostengr.HpLightShow.WebApi.Models;

public sealed class AppSettings
{
    public List<string> ChristmasHashTags { get; init; } = null;
    public List<string> IndependenceDayHashTags { get; init; } = null;
    public string FppApiUrl { get; init; } = "http://10.10.50.101";
    public string SequenceOverride { get; set; } = null;
    public double MaxCpuTemperatureC { get; init; } = 60.0;

    public DateOnly EasterStartDate { get; init; } = new(DateTime.Now.Year, 3, 26);
    public DateOnly EasterEndDate { get; init; } = new(DateTime.Now.Year, 3, 28);
    public DateOnly FatTuesdayDate { get; init; } = new(DateTime.Now.Year, 2, 28);
    public int MonitorDelay { get; init; } = 5;
}
