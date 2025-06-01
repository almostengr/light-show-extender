using Almostengr.Common.DomainServices.Resources;

namespace Almostengr.HpLightShow.WebApi.Features.Monitoring.DomainServices.Resources;

public sealed class WledStatusResource : BaseResource
{
    public WledState State { get; set; } = new();


    public sealed class WledState
    {
        public bool On { get; set; }
    }
}
