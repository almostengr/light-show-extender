using System.Collections;
using RhtServices.Common.Utilities.DomainService;

namespace RhtServices.HpLightShow.Core.DomainHandler.ShowCountdown;

public sealed class ShowCountdownHandler : IHandler<ShowCountdownDto, HandlerResult>
{
    public async Task<HandlerResult> ExecuteAsync(ShowCountdownDto countdownDto)
    {
        // todo separate dates for Christmas and 4th of July in configuraiton file

        if (countdownDto.IsChrismtas)
        {
            await ChristmasCountdownAsync(countdownDto);
            return new HandlerResult(true);
        }

        await IndependenceCountdownAsync(countdownDto);
        return new HandlerResult(true);
    }

    private async Task ChristmasCountdownAsync(ShowCountdownDto countdownDto)
    {

    }

    private async Task IndependenceCountdownAsync(ShowCountdownDto countdownDto)
    {

    }
}