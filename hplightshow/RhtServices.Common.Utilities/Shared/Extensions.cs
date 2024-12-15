namespace RhtServices.Common.Utilities.Shared;

public static class Extensions
{
    public static float ToFahrenheitFromCelsius(this float celsius)
    {
        float fahrenheit = (((float)celsius * 9) / 5) + 32;
        return fahrenheit;
    }

    public static bool IsNotNullOrWhiteSpace(this string value)
    {
        return !string.IsNullOrWhiteSpace(value);
    }

    public static DateTime GetNthWeekdayOfMonth(int year, int month, DayOfWeek dayOfWeek, int occurrence)
    {
        DateTime firstDayOfMonth = new DateTime(year, month, 1);
        int daysOffset = ((int)dayOfWeek - (int)firstDayOfMonth.DayOfWeek + 7) % 7;
        DateTime firstOccurrence = firstDayOfMonth.AddDays(daysOffset);

        return firstOccurrence.AddDays((occurrence - 1) * 7);
    }
}