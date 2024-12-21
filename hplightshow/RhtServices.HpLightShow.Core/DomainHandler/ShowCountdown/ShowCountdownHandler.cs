using RhtServices.Common.Utilities.DomainService;

namespace RhtServices.HpLightShow.Core.DomainHandler.ShowCountdown;

public sealed class ShowCountdownHandler : IHandler<ShowCountdownRequest, ShowCountdownResult>
{
    public Task<ShowCountdownResult> ExecuteAsync(ShowCountdownRequest request)
    {
        throw new NotImplementedException();
    }
}