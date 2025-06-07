using Almostengr.Common.DomainServices.Resources;

namespace Almostengr.HpLightShow.WebApi.Features.Monitoring.DomainServices.Resources;

public class StatusMessageResource : BaseResource
{
    public string Message { get; set; }
    public string Status { get; set; }
}