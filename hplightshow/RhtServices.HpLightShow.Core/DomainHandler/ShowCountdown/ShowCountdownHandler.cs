using System.Collections;
using RhtServices.Common.Utilities.DomainService;

namespace RhtServices.HpLightShow.Core.DomainHandler.ShowCountdown;

public sealed class ShowCountdownHandler : IHandler<ShowCountdownRequest, ShowCountdownResult>
{
    public async Task<ShowCountdownResult> ExecuteAsync(ShowCountdownRequest request)
    {
        // todo separate dates for Christmas and 4th of July in configuraiton file

        if (request.IsChrismtas)
        {
            await ChristmasCountdownAsync(request);
            return new ShowCountdownResult(true);
        }

        await IndependenceCountdownAsync(request);
        return new ShowCountdownResult(true);
    }

    private async Task ChristmasCountdownAsync(ShowCountdownRequest request)
    {

    }

    private async Task IndependenceCountdownAsync(ShowCountdownRequest request)
    {

    }
}