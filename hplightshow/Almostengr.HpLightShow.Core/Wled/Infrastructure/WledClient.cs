using Almostengr.Common.Infrastructure;
using Almostengr.HpLightShow.Core.Wled.Resources;

namespace Almostengr.HpLightShow.Core.Wled.Infrastructure;

public sealed class WledClient : IWledClient
{
    private readonly HttpClient _httpClient;

    public WledClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.Timeout = TimeSpan.FromSeconds(15);
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    public async Task<WledStatusResource> GetStatusAsync(string hostname)
    {
        _ = hostname ?? throw new ArgumentNullException(nameof(hostname));

        string route = hostname + "/json/status";
        var response = await _httpClient.GetAsync<WledStatusResource>(route);
        return response;
    }

    public async Task<WledStatusResource> UpdateStatusAsync(WledStatusResource resource, string hostname)
    {
        _ = resource ?? throw new ArgumentNullException(nameof(resource));
        _ = hostname ?? throw new ArgumentNullException(nameof(hostname));

        string route = hostname + "/json/status";
        var response = await _httpClient.PostAsync<WledStatusResource, WledStatusResource>(route, resource);
        return response;
    }
}