using Almostengr.LightShowExtender.DomainService.Wled;
using Almostengr.Extensions;

namespace Almostengr.LightShowExtender.Infrastructure.Wled;

public sealed class WledHttpClient : IWledHttpClient
{
    private readonly HttpClient _httpClient;

    public WledHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<WledJsonResponse> GetStatusAsync(string hostname, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(hostname))
        {
            throw new ArgumentNullException(nameof(hostname));
        }

        string route = $"{hostname}/json";
        return await _httpClient.GetAsync<WledJsonResponse>(route.GetUrlWithProtocol(), cancellationToken);
    }

    public async Task<WledJsonResponse> PostStateAsync(string hostname, WledJsonStateRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(hostname))
        {
            throw new ArgumentNullException(nameof(hostname));
        }

        string route = $"{hostname}/json/state";
        return await _httpClient.PostAsync<WledJsonStateRequest, WledJsonResponse>(route.GetUrlWithProtocol(), request, cancellationToken);
    }
}