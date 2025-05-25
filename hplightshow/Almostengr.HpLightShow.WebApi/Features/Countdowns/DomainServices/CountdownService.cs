using Almostengr.Common.DomainServices.Results;
using Almostengr.HpLightShow.Core.Countdowns.DataTransferObjects;
using Almostengr.HpLightShow.Core.SocialMedias.DomainServices;
using Microsoft.Extensions.Logging;

namespace Almostengr.HpLightShow.Core.Countdowns.Service;

public sealed class CountdownService : ICountdownService
{
    private readonly ISocialMediaPoster _socialMediaPoster;
    private readonly ILogger<CountdownService> _logger;

    public CountdownService(
        ISocialMediaPoster socialMediaPoster,
        ILogger<CountdownService> logger)
    {
        _socialMediaPoster = socialMediaPoster;
        _logger = logger;
    }

    public async Task<Result<HolidayCountdownResource>> ExecuteAsync(HolidayCountdownResource resource, bool commitTransaction = true)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Result<HolidayCountdownResource>.Failure(ex);
        }
    }
}
