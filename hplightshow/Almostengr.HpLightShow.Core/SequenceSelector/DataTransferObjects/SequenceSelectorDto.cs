namespace Almostengr.HpLightShow.Core.SequenceSelector.DataTransferObjects;

public sealed class SequenceSelectorDto
{
    public SequenceSelectorDto(DateOnly currentDate)
    {
        CurrentDate = currentDate;
    }

    public DateOnly CurrentDate { get; init; }
}