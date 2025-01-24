using Almostengr.Common.OperationResult;
using Almostengr.HpLightShow.Core.Common.Common;

namespace Almostengr.HpLightShow.Core.Countdowns;

public sealed class CountdownService : ICountdownService
{
    private readonly ISocialMediaPoster _socialMediaPoster;

    public CountdownService(ISocialMediaPoster socialMediaPoster)
    {
        _socialMediaPoster = socialMediaPoster;
    }

    public async Task<Result<int>> PostCountdownAsync(HolidayCountdownDto countdownDto)
    {
        _ = countdownDto ?? throw new ArgumentNullException(nameof(countdownDto));

        int daysDifference = countdownDto.HolidayDate.DayNumber - countdownDto.CurrentDate.DayNumber;
        string? message;

        Result<int> result = Result<int>.Create();

        if (daysDifference > 0)
        {
            message = $"{daysDifference} day(s) until {countdownDto.HolidayName}.";
        }
        else if (daysDifference == 0)
        {
            message = $"Today is {countdownDto.HolidayName}!";
        }
        else { 
            result.AddError("Date difference was negative. Are the days entered backwards?");
            return result;
        }

        if (string.IsNullOrWhiteSpace(message))
        {
            return result;
        }

        await _socialMediaPoster.PostAsync(message);
        return result;
    }
}
