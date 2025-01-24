using Almostengr.Common.OperationResult;
using Almostengr.HpLightShow.Core.Common.Common;
using Almostengr.HpLightShow.Core.Countdowns.DataTransferObjects;

namespace Almostengr.HpLightShow.Core.Countdowns.Service;

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

        if (daysDifference > 0)
        {
            message = $"{daysDifference} day(s) until {countdownDto.HolidayName}.";
        }
        else if (daysDifference == 0)
        {
            message = $"Today is {countdownDto.HolidayName}!";
        }
        else
        {
            return Result<int>.Failure("Date difference was negative. Are the days entered backwards?");
        }

        if (string.IsNullOrWhiteSpace(message))
        {
            return Result<int>.Success(0);
        }

        await _socialMediaPoster.PostAsync(message);
        return Result<int>.Success(0);
    }
}
