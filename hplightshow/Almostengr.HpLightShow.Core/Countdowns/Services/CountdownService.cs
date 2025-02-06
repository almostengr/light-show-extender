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

    public async Task<Result<HolidayCountdownResource>> ExecuteAsync(HolidayCountdownResource resource)
    {
        _ = resource ?? throw new ArgumentNullException(nameof(resource));

        int daysDifference = resource.HolidayDate.DayNumber - resource.CurrentDate.DayNumber;
        string? message;

        if (daysDifference > 0)
        {
            message = $"{daysDifference} day(s) until {resource.HolidayName}.";
        }
        else if (daysDifference == 0)
        {
            message = $"Today is {resource.HolidayName}!";
        }
        else
        {
            return Result<HolidayCountdownResource>.Failure("Date difference was negative. Are the days entered backwards?");
        }

        if (string.IsNullOrWhiteSpace(message))
        {
            return Result<HolidayCountdownResource>.Success(resource);
        }

        await _socialMediaPoster.PostAsync(message);
        return Result<HolidayCountdownResource>.Success(resource);
    }
}
