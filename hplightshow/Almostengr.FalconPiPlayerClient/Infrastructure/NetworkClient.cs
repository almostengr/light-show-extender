using Almostengr.FalconPiPlayerClient.DomainServices.Interfaces;
using Almostengr.FalconPiPlayerClient.DomainServices.NetworkResources;

namespace Almostengr.FalconPiPlayerClient.Infrastructure;

public sealed class NetWorkClient : BaseClient, INetworkHttpClient
{
    public NetWorkClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public async Task<FppDnsResource> GetDnsAsync()
    {
        var response = await _httpClient.GetAsync("api/network/dns");
        var result = await DeserializeResponseBodyAsync<FppDnsResource>(response);
        return result;
    }

    public async Task<FppDnsOutputResource> PutDnsAsync(FppDnsResource request)
    {
        var json = SerializeRequestBody<FppDnsResource>(request);
        var response = await _httpClient.PutAsync("api/network/dns", json);
        var result = await DeserializeResponseBodyAsync<FppDnsOutputResource>(response);
        return result;
    }

    public Task<List<NetworkInterfaceResource>> GetInterfacesAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<NamedNetworkInterfaceResource> GetInterfaceByNameAsync(string name)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(name);

        string route = $"api/network/interface/{name}";
        var response = await _httpClient.GetAsync(route);
        var reuslt = await DeserializeResponseBodyAsync<NamedNetworkInterfaceResource>(response);
        return reuslt;
    }

    public async Task<StatusOnlyResource> UpdateInterfaceByNameAsync(CoreNetworkInterfaceResource request, string name)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(name);

        string route = $"api/network/interface/{name}";
        var json = SerializeRequestBody<CoreNetworkInterfaceResource>(request);
        var response = await _httpClient.PostAsync(route, json);
        var result = await DeserializeResponseBodyAsync<StatusOnlyResource>(response);
        return result;
    }

    public async Task<StatusOnlyResource> ApplyInterfaceByNameAsync(string name)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(name);

        string route = $"api/network/interface/{name}/apply";
        var response = await _httpClient.PostAsync(route, null);
        var result = await DeserializeResponseBodyAsync<StatusOnlyResource>(response);
        return result;
    }

    public async Task<StatusOnlyResource> DeletePersistentNamesAsync()
    {
        var response = await _httpClient.DeleteAsync("api/netwrok/presistentNames");
        var result = await DeserializeResponseBodyAsync<StatusOnlyResource>(response);
        return result;
    }

    public async Task<StatusOnlyResource> CreatePersistentNamesAsync()
    {
        var response = await _httpClient.PostAsync("api/network/presistentNames", null);
        var result = await DeserializeResponseBodyAsync<StatusOnlyResource>(response);
        return result;
    }

    public async Task<NetworkResource> GetWifiInterfacesAsync(string name)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(name);

        var response = await _httpClient.GetAsync($"api/network/wifi/scan/{name}");
        var result = await DeserializeResponseBodyAsync<NetworkResource>(response);
        return result;
    }

    public async Task<WifiStrengthResource> GetWifiStrengthAsync()
    {
        var response = await _httpClient.GetAsync("api/network/wifi/strength");
        var result = await DeserializeResponseBodyAsync<WifiStrengthResource>(response);
        return result;
    }
}
