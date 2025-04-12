using Almostengr.Common.DomainServices.Results;
using Almostengr.HpLightShow.Core.Countdowns.DataTransferObjects;
using Almostengr.HpLightShow.Core.SocialMedias.DomainServices;

namespace Almostengr.HpLightShow.Core.Countdowns.Service;

public sealed class CountdownService : ICountdownService
{
    private readonly ISocialMediaPoster _socialMediaPoster;

    public CountdownService(ISocialMediaPoster socialMediaPoster)
    {
        _socialMediaPoster = socialMediaPoster;
    }

    public async Task<Result<HolidayCountdownResource>> ExecuteAsync(HolidayCountdownResource resource, bool commitTransaction = true)
    {
        ArgumentNullException.ThrowIfNull(resource, nameof(resource));

        int daysDifference = resource.HolidayDate.DayNumber - resource.CurrentDate.DayNumber;
        string? message = null;

        if (daysDifference > 0)
        {
            message = $"{daysDifference} day(s) until {resource.HolidayName}.";
        }
        else if (daysDifference == 0)
        {
            message = $"Today is {resource.HolidayName}!";
        }

        if (!string.IsNullOrWhiteSpace(message))
        {
            await _socialMediaPoster.PostAsync(message);
        }

        return Result<HolidayCountdownResource>.Success(resource);
    }
}
