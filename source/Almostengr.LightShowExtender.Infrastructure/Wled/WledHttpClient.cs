using Almostengr.LightShowExtender.DomainService.Wled;
using Almostengr.HttpClient;

namespace Almostengr.LightShowExtender.Infrastructure.Wled;

public sealed class WledHttpClient : BaseHttpClient, IWledHttpClient
{
    public WledHttpClient()
    {
        _httpClient = new HttpClient();
    }

    public async Task<WledJsonResponse> GetStatusAsync(string hostname, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(hostname))
        {
            throw new ArgumentNullException(nameof(hostname));
        }

        string route = $"{hostname}/json";
        return await HttpGetAsync<WledJsonResponse>(route, cancellationToken);
    }

    public async Task<WledJsonResponse> PostStateAsync(string hostname, WledJsonStateRequest wledRequestDto, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(hostname))
        {
            throw new ArgumentNullException(nameof(hostname));
        }

        string route = $"{hostname}/json/state";
        return await HttpPostAsync<WledJsonStateRequest, WledJsonResponse>(route, wledRequestDto, cancellationToken);
    }
}