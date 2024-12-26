namespace RhtServices.HpLightShow.Core.DomainHandler.SequenceSelector;

internal sealed class SequenceRule
{
    public SequenceRule(DateOnly startDate, DateOnly endDate, LightingSequence sequence, List<DayOfWeek>? daysOfWeek = null)
    {
        StartDate = startDate;
        EndDate = endDate;
        DaysOfWeek = daysOfWeek;
        Sequence = sequence;
    }

    public DateOnly StartDate { get; init; }
    public DateOnly EndDate { get; init; }
    public LightingSequence Sequence { get; init; }
    public List<DayOfWeek>? DaysOfWeek { get; init; }
}