using Almostengr.Wled.DomainService
using Almostengr.Extensions;

namespace Almostengr.LightShowExtender.Infrastructure.Wled;

public sealed class WledHttpClient : IWledHttpClient
{
    private readonly HttpClient _httpClient;

    public WledHttpClient()
    {
        _httpClient = new HttpClient();
    }

    public async Task<WledJsonStateResponse> GetStatusAsync(string hostname, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(hostname))
        {
            throw new ArgumentNullException(nameof(hostname));
        }

        string route = $"{hostname}/json";
        return await _httpClient.GetAsync<WledJsonStateResponse>(route.GetUrlWithProtocol(), cancellationToken);
    }

    public async Task<WledJsonStateResponse> PostStateAsync(string hostname, WledJsonStateRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(hostname))
        {
            throw new ArgumentNullException(nameof(hostname));
        }

        string route = $"{hostname}/json/state";
        return await _httpClient.PostAsync<WledJsonStateRequest, WledJsonStateResponse>(route.GetUrlWithProtocol(), request, cancellationToken);
    }
}