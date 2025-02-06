using Almostengr.Common.Resources;

namespace Almostengr.HpLightShow.Core.Countdowns.DataTransferObjects;

public class HolidayCountdownResource : BaseResource
{
    public HolidayCountdownResource(DateOnly currentDate, DateOnly holidayDate, string dayOfMessage, string holidayName)
    {
        if (string.IsNullOrWhiteSpace(dayOfMessage))
        {
            throw new ArgumentNullException(nameof(dayOfMessage));
        }

        if (string.IsNullOrWhiteSpace(holidayName))
        {
            throw new ArgumentNullException(nameof(holidayName));
        }

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
