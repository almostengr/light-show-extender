namespace Almostengr.HpLightShow.Core.Countdowns;

public interface ICountdownService
{
    Task PostCountdownAsync(HolidayCountdownDto countdownDto);
}