using Almostengr.FalconPiPlayerClient.DomainServices.NetworkResources;

namespace Almostengr.FalconPiPlayerClient.DomainServices.Interfaces;

public interface INetworkHttpClient
{
    Task<FppDnsResource> GetDnsAsync();
    Task<FppDnsOutputResource> PutDnsAsync(FppDnsResource request);
}
