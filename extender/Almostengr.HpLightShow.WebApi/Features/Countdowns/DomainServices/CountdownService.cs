using Almostengr.Common.DomainServices.Results;
using Almostengr.HpLightShow.Core.Countdowns.DomainServices.Resources;
using Almostengr.HpLightShow.WebApi.Countdowns.DomainServices.Interfaces;
using Almostengr.HpLightShow.WebApi.Features.SocialMediaPosts.DomainServices.Interfaces;
using Almostengr.HpLightShow.WebApi.Features.SocialMediaPosts.DomainServices.Resources;

namespace Almostengr.HpLightShow.Core.Countdowns.Service;

public sealed class CountdownService : ICountdownService
{
    private readonly ISocialMediaPosterService _socialMediaPosterService;
    private readonly ILogger<CountdownService> _logger;

    public CountdownService(
        ILogger<CountdownService> logger,
        ISocialMediaPosterService socialMediaPosterService
        )
    {
        _logger = logger;
        _socialMediaPosterService = socialMediaPosterService;
    }

    public async Task<Result<HolidayCountdownResource>> ExecuteAsync(HolidayCountdownResource resource, bool commitTransaction = true)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(resource, nameof(resource));

            int daysDifference = resource.HolidayDate.DayNumber - resource.CurrentDate.DayNumber;
            string message = null;

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
                Result<SocialMediaResource> socialResult = await _socialMediaPosterService.PostAsync(message);
                if (socialResult.Failed)
                {
                    return Result<HolidayCountdownResource>.Failure(socialResult.Errors);
                }
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
