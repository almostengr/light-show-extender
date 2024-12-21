using RhtServices.Common.Utilities.DomainService;

namespace RhtServices.HpLightShow.Core.DomainHandler.ShowCountdown;

public sealed class ShowCountdownResult : HandlerResult, IHandlerResult
{
    public ShowCountdownResult(bool succeeded) : base(succeeded)
    {
    }
}