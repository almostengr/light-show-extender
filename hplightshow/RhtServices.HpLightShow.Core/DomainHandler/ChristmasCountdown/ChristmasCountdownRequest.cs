using RhtServices.Common.Utilities.DomainService;

namespace RhtServices.HpLightShow.Core.DomainHandler.ChristmasCountdown;

public sealed class ChristmasCountdownRequest : IHandlerRequest
{
    public ChristmasCountdownRequest(DateOnly christmasDate, DateOnly currentDate)
    {
        CurrentDate = currentDate;
        ChristmasDate = christmasDate;
    }

    public DateOnly CurrentDate { get; init; }
    public DateOnly ChristmasDate { get; init; }
}