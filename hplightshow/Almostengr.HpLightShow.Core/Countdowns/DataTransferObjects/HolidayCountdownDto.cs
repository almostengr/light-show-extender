namespace Almostengr.HpLightShow.Core.Countdowns;

public class HolidayCountdownDto
{
    public HolidayCountdownDto(DateOnly currentDate, DateOnly holidayDate, string dayOfMessage, string holidayName)
    {
        if (string.IsNullOrWhiteSpace(dayOfMessage) || string.IsNullOrWhiteSpace(holidayName))
        {
            throw new ArgumentNullException();
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
