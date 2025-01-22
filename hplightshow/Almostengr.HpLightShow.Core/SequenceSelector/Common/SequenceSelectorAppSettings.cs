namespace Almostengr.HpLightShow.Core.SequenceSelector.Common;

public sealed class SequenceSelectorAppSettings
{
    public string? SequenceOverride { get; init; } = null;
    public DateOnly EasterStartDate { get; init; } = new(DateTime.Now.Year, 3, 26);
    public DateOnly EasterEndDate { get; init; } = new(DateTime.Now.Year, 3, 28);
    public DateOnly FatTuesdayDate { get; init; } = new(DateTime.Now.Year, 2, 28);
}