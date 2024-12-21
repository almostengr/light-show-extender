using RhtServices.Common.Utilities.DomainService;

namespace RhtServices.HpLightShow.Core.DomainHandler.ShowCountdown;

public sealed class ShowCountdownRequest : IHandlerRequest
{
    public DateTime CurrentDate { get; set; }
}