using Almostengr.FalconPiPlayerClient.DomainServices.NetworkResources;

namespace Almostengr.FalconPiPlayerClient.DomainServices.Interfaces;

public interface INetworkHttpClient
{
    Task<FppDnsResource> GetDnsAsync();
    Task<FppDnsOutputResource> PutDnsAsync(FppDnsResource request);
    Task<NamedNetworkInterfaceResource> GetInterfaceByNameAsync(string name);
    Task<StatusOnlyResource> UpdateInterfaceByNameAsync(CoreNetworkInterfaceResource request, string name);
    Task<StatusOnlyResource> ApplyInterfaceByNameAsync(string name);
    Task<StatusOnlyResource> DeletePersistentNamesAsync();
    Task<StatusOnlyResource> CreatePersistentNamesAsync();
    Task<NetworkResource> GetWifiInterfacesAsync(string name);
    Task<WifiStrengthResource> GetWifiStrengthAsync();
}
