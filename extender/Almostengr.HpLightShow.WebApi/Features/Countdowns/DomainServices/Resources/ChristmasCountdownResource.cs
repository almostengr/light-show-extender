namespace Almostengr.HpLightShow.Core.Countdowns.DomainServices.Resources;

public sealed class ChristmasCountdownResource : HolidayCountdownResource
{
    public ChristmasCountdownResource(DateOnly currentDate) :
        base(currentDate, new DateOnly(DateTime.Now.Year, 12, 25), "Today is Christmas!", "Christmas")
    {
    }
}
