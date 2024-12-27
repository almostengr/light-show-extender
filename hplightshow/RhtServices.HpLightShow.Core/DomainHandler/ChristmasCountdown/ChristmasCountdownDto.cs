using RhtServices.Common.Utilities.DomainService;

namespace RhtServices.HpLightShow.Core.DomainHandler.ChristmasCountdown;

public sealed class ChristmasCountdownDto : IHandlerDto
{
    public ChristmasCountdownDto(DateOnly christmasDate, DateOnly currentDate)
    {
        CurrentDate = currentDate;
        ChristmasDate = christmasDate;
    }

    public DateOnly CurrentDate { get; init; }
    public DateOnly ChristmasDate { get; init; }
}