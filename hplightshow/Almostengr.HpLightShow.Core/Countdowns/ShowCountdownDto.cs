using Almostengr.Common.Utilities.DomainService;

namespace Almostengr.HpLightShow.Core.DomainHandler.ShowCountdown;

public sealed class ShowCountdownDto : IHandlerDto
{
    public DateTime CurrentDate { get; set; }
    public DateTime NextShowDate { get; set; }
    public bool IsChrismtas { get; set; }
}