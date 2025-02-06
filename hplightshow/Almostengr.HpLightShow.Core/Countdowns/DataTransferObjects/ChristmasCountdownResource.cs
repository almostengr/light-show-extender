namespace Almostengr.HpLightShow.Core.Countdowns.DataTransferObjects;

public sealed class ChristmasCountdownResource : HolidayCountdownResource
{
    public ChristmasCountdownResource(DateOnly currentDate) :
        base(currentDate, new DateOnly(DateTime.Now.Year, 12, 25), "Today is Christmas!", "Christmas")
    {
    }
}