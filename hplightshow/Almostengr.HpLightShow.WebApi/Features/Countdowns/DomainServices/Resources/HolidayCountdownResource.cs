using Almostengr.Common.DomainServices.Resources;

namespace Almostengr.HpLightShow.Core.Countdowns.DomainServices.Resources;

public class HolidayCountdownResource : BaseResource
{
    public HolidayCountdownResource(DateOnly currentDate, DateOnly holidayDate, string dayOfMessage, string holidayName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(dayOfMessage);
        ArgumentException.ThrowIfNullOrWhiteSpace(holidayName);

        CurrentDate = currentDate;
        HolidayDate = holidayDate;
        DayOfMessage = dayOfMessage;
        HolidayName = holidayName;
    }

    public DateOnly CurrentDate { get; init; }
    public DateOnly HolidayDate { get; init; }
    public string DayOfMessage { get; init; }
    public string HolidayName { get; init; }
}
