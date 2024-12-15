using RhtServices.Common.Utilities.DomainService;

namespace RhtServices.HpLightShow.Core.DomainHandler.ChristmasCountdown;

public sealed class ChristmasCountdownResult : HandlerResult, IHandlerResult
{
    public ChristmasCountdownResult(bool succeeded): base(succeeded)
    {
    }
}