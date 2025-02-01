namespace Almostengr.HpLightShow.Core.re;

public sealed class SequenceSelectorResource
{
    public SequenceSelectorResource(DateOnly currentDate)
    {
        CurrentDate = currentDate;
    }

    public DateOnly CurrentDate { get; init; }
}