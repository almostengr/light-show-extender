namespace Almostengr.Common.Extensions;

public static class Calendar
{
    public static DateTime GetNthWeekdayOfMonth(int year, int month, DayOfWeek dayOfWeek, int occurrence)
    {
        DateTime firstDayOfMonth = new DateTime(year, month, 1);
        int daysOffset = ((int)dayOfWeek - (int)firstDayOfMonth.DayOfWeek + 7) % 7;
        DateTime firstOccurrence = firstDayOfMonth.AddDays(daysOffset);

        return firstOccurrence.AddDays((occurrence - 1) * 7);
    }
}