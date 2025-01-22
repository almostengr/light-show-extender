namespace Almostengr.HpLightShow.Core.FalconPiPlayer.DataTransferObjects;

public sealed class SequenceSelectorDto
{
    public SequenceSelectorDto(DateOnly currentDate)
    {
        CurrentDate = currentDate;
    }

    public DateOnly CurrentDate { get; init; }
}