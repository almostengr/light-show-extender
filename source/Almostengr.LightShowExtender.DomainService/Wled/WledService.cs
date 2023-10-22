using Almostengr.Extensions;
using Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

namespace Almostengr.LightShowExtender.DomainService.Wled;

public sealed class WledService : IWledService
{
    private readonly HttpClient _httpClient;

    public WledService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<WledJsonResponse> TurnOnAsync(string hostname, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(hostname))
        {
            throw new ArgumentNullException(nameof(hostname));
        }

        var request = new WledJsonStateRequest(true);
        var route = $"{hostname}/json/state";
        return await _httpClient.PostAsync<WledJsonStateRequest, WledJsonResponse>(route, request, cancellationToken);
    }

    public async Task<WledJsonResponse> TurnOffAsync(string hostname, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(hostname))
        {
            throw new ArgumentNullException(nameof(hostname));
        }

        var request = new WledJsonStateRequest(false);
        var route = $"{hostname}/json/state";
        return await _httpClient.PostAsync<WledJsonStateRequest, WledJsonResponse>(route, request, cancellationToken);
    }

    public async Task TurnOffAsync(List<string> hostnames, CancellationToken cancellationToken)
    {
        foreach(var hostname in hostnames)
        {
            await TurnOffAsync(hostname, cancellationToken);
        }
    }

    public async Task TurnOnAsync(List<string> hostnames, CancellationToken cancellationToken)
    {
        foreach(var hostname in hostnames)
        {
            await TurnOffAsync(hostname, cancellationToken);
        }
    }
}

public interface IWledService
{
    Task<WledJsonResponse> TurnOffAsync(string hostname, CancellationToken cancellationToken);
    Task TurnOffAsync(List<string> hostnames, CancellationToken cancellationToken);
    Task<WledJsonResponse> TurnOnAsync(string hostname, CancellationToken cancellationToken);
    Task TurnOnAsync(List<string> hostnames, CancellationToken cancellationToken);
}