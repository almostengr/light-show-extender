namespace Almostengr.HpLightShow.WebApi.Models;

public sealed class AppSettings
{
    public List<string> ChristmasHashTags { get; set; } = null;
    public List<string> IndependenceDayHashTags { get; set; } = null;
    public DateOnly FatTuesdayDate { get; set; }
    public DateOnly EasterStartDate { get; set; }
    public DateOnly EasterEndDate { get; set; }
    public string SequenceOverride { get; set; } = null;
}
