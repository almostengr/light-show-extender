using Almostengr.HpLightShow.WebApi.Features.Monitoring.DomainServices.Resources;

namespace Almostengr.HpLightShow.WebApi.Features.Monitoring.DomainServices.Interfaces;

public interface IWledClient
{
    public Task<WledStatusResource> GetStatusAsync(string hostname);
    public Task<WledStatusResource> UpdateStatusAsync(WledStatusResource resource, string hostname);
}