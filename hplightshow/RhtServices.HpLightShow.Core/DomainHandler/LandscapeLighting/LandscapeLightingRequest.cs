using RhtServices.Common.Utilities.DomainService;

namespace RhtServices.HpLightShow.Core.DomainHandler.LandscapeLighting;

public sealed class LandscapeLightingRequest : IHandlerRequest
{
    public LandscapeLightingRequest(DateOnly currentDate)
    {
        CurrentDate = currentDate;
    }

    public DateOnly CurrentDate { get; init; }
}