using Almostengr.Extensions;

namespace Almostengr.LightShowExtender.DomainService.Wled;

public sealed class WledService : IWledService
{
    private readonly IWledHttpClient _httpClient;

    public WledService(IWledHttpClient httpClient)
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
        return await _httpClient.PostStateAsync(hostname, request, cancellationToken);
    }

    public async Task<WledJsonResponse> TurnOffAsync(string hostname, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(hostname))
        {
            throw new ArgumentNullException(nameof(hostname));
        }

        var request = new WledJsonStateRequest(false);
        return await _httpClient.PostStateAsync(hostname, request, cancellationToken);
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