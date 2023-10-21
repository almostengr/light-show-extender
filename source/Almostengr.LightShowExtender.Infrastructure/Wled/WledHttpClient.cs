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
        var response = await _httpClient.GetAsync(route, cancellationToken);
        await response.WasRequestSuccessfulAsync(cancellationToken);
        return await response.DeserializeResponseBodyAsync<WledJsonResponse>(cancellationToken);
    }

    public async Task<WledJsonResponse> PostStateAsync(string hostname, WledJsonStateRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(hostname))
        {
            throw new ArgumentNullException(nameof(hostname));
        }

        string route = $"{hostname}/json/state";
        var response = await _httpClient.PostAsync(route, request.SerializeRequestBodyAsync<WledJsonStateRequest>(), cancellationToken);
        await response.WasRequestSuccessfulAsync(cancellationToken);
        return await response.DeserializeResponseBodyAsync<WledJsonResponse>(cancellationToken);
    }
}