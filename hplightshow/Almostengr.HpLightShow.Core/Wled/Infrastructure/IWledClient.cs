using Almostengr.HpLightShow.Core.Wled.Resources;

namespace Almostengr.HpLightShow.Core.Wled.Infrastructure;

public interface IWledClient
{
    public Task<WledStatusResource> GetStatusAsync(string hostname);
    public Task<WledStatusResource> UpdateStatusAsync(WledStatusResource resource, string hostname);
}