using Almostengr.Common.DomainServices.Resources;

namespace Almostengr.HpLightShow.WebApi.Features.Monitoring.DomainServices.Resources;

public sealed class MonitorResource : BaseResource
{
    public string PrimaryHostname { get; set; }
    public double MaxCpuTemperatureC { get; set; } = 60.0;
}

