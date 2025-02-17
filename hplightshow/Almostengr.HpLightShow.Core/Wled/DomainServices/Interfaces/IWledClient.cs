namespace Almostengr.HpLightShow.Core.Wled.DomainServices.Interfaces;

public interface IWledClient
{
    public Task<WledStatusResource> GetStatusAsync(string hostname);
    public Task<WledStatusResource> UpdateStatusAsync(WledStatusResource resource, string hostname);
}