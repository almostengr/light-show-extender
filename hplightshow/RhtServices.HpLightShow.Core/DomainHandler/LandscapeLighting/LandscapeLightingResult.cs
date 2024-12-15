using RhtServices.Common.Utilities.DomainService;

namespace RhtServices.HpLightShow.Core.DomainHandler.LandscapeLighting;

public sealed class LandscapeLightingResult : HandlerResult, IHandlerResult
{
    public LandscapeLightingResult(bool succeeded) : base(succeeded)
    {
    }
}