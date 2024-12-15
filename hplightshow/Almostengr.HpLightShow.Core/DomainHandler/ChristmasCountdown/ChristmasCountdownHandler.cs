using Almostengr.Common.Utilities.DomainService;
using Almostengr.HpLightShow.Core.Common.DomainHandler.Common;

namespace Almostengr.HpLightShow.Core.DomainHandler.ChristmasCountdown;

public sealed class ChristmasCountdownHandler : IHandler<ChristmasCountdownRequest, ChristmasCountdownResult>
{
    private readonly ISocialMediaPoster _socialMediaPoster;

    public ChristmasCountdownHandler(ISocialMediaPoster socialMediaPoster)
    {
        _socialMediaPoster = socialMediaPoster;
    }

    public async Task<ChristmasCountdownResult> ExecuteAsync(ChristmasCountdownRequest request)
    {
        string? message = null;

        int daysDifference = request.ChristmasDate.DayNumber - request.CurrentDate.DayNumber;
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

        return await Task.FromResult<ChristmasCountdownResult>(new ChristmasCountdownResult(true));
    }
}