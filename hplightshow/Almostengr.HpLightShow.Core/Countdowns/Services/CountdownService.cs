using Almostengr.HpLightShow.Core.Common.Common;

namespace Almostengr.HpLightShow.Core.Countdowns;

public sealed class CountdownService : ICountdownService
{
    private readonly ISocialMediaPoster _socialMediaPoster;

    public CountdownService(ISocialMediaPoster socialMediaPoster)
    {
        _socialMediaPoster = socialMediaPoster;
    }

    public async Task PostCountdownAsync(HolidayCountdownDto countdownDto)
    {
        _ = countdownDto ?? throw new ArgumentNullException(nameof(countdownDto));

        int daysDifference = countdownDto.HolidayDate.DayNumber - countdownDto.CurrentDate.DayNumber;
        string? message = null;
        if (daysDifference > 0)
        {
            message = $"{daysDifference} day(s) until {countdownDto.HolidayName}.";
        }
        else if (daysDifference == 0)
        {
            message = $"Today is {countdownDto.HolidayName}!";
        }

        if (string.IsNullOrWhiteSpace(message))
        {
            return;
        }

        await _socialMediaPoster.PostAsync(message);
    }
}
