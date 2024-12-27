using RhtServices.Common.Utilities.DomainService;
using RhtServices.HpLightShow.Core.Common.DomainHandler.Common;

namespace RhtServices.HpLightShow.Core.DomainHandler.ChristmasCountdown;

public sealed class ChristmasCountdownHandler : IHandler<ChristmasCountdownDto, HandlerResult>
{
    private readonly ISocialMediaPoster _socialMediaPoster;

    public ChristmasCountdownHandler(ISocialMediaPoster socialMediaPoster)
    {
        _socialMediaPoster = socialMediaPoster;
    }

    public async Task<HandlerResult> ExecuteAsync(ChristmasCountdownDto countdownDto)
    {
        string? message = null;

        int daysDifference = countdownDto.ChristmasDate.DayNumber - countdownDto.CurrentDate.DayNumber;
        if (daysDifference > 0)
        {
            message = daysDifference == 1 ? $"1 day " : $"{daysDifference} days ";
        }
        else if (daysDifference == 0)
        {
            message = "Today is Christmas!";
        }

        if (!string.IsNullOrWhiteSpace(message))
        {
            await _socialMediaPoster.PostAsync(message);
        }

        return new HandlerResult(true);
    }
}