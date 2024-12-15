namespace Almostengr.HpLightShow.DomainService.ChristmasCountdown;

public sealed class ChristmasCountdownResult : IHandlerResult
{
    public ChristmasCountdownResult(bool succeeded)
    {
        Succeeded = succeeded;
    }

    public bool Succeeded { get; init; }
}