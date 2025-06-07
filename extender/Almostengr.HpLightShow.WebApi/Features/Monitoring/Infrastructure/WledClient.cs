using Almostengr.Common.Infrastructure;
using Almostengr.HpLightShow.WebApi.Features.Monitoring.DomainServices.Interfaces;
using Almostengr.HpLightShow.WebApi.Features.Monitoring.DomainServices.Resources;

namespace Almostengr.HpLightShow.WebApi.Features.Monitoring.Infrastructure;

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
        ArgumentException.ThrowIfNullOrWhiteSpace(hostname, nameof(hostname));

        string route = hostname + "/json/status";
        var response = await _httpClient.GetAsync(route);
        var result = await response.DeserializeResponseBodyAsync<WledStatusResource>();
        return result;
    }

    public async Task<WledStatusResource> UpdateStatusAsync(WledStatusResource resource, string hostname)
    {
        ArgumentNullException.ThrowIfNull(resource, nameof(resource));
        ArgumentException.ThrowIfNullOrWhiteSpace(hostname, nameof(hostname));

        string route = hostname + "/json/status";
        var json = resource.SerializeRequestBody();
        var response = await _httpClient.PostAsync(route, json);
        var result = await response.DeserializeResponseBodyAsync<WledStatusResource>();
        return result;
    }
}