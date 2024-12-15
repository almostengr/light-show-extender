using Almostengr.Common.Utilities.DomainService;

namespace Almostengr.HpLightShow.Core.DomainHandler.ChristmasCountdown;

public sealed class ChristmasCountdownResult : HandlerResult, IHandlerResult
{
    public ChristmasCountdownResult(bool succeeded): base(succeeded)
    {
    }
}