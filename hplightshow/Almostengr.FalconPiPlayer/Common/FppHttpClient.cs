using Almostengr.Common.Utilities.Infrastructure;
using Almostengr.FalconPiPlayer.DomainService;

namespace Almostengr.FalconPiPlayer.Common;

public sealed class FppHttpClient : IFppHttpClient
{
    private readonly HttpClient _httpClient;

    public FppHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("http://127.0.0.1");
        _httpClient.Timeout = TimeSpan.FromSeconds(15);
    }

    public async Task<FppStatusResult> GetFppdStatusAsync()
    {
        string route = "api/fppd/status";
        return await _httpClient.GetAsync<FppStatusResult>(route);
    }

    public async Task<MultiSyncSystemsQueryResponse> GetMultiSyncSystemsAsync()
    {
        string route = "api/fppd/multiSyncSystems";
        return await _httpClient.GetAsync<MultiSyncSystemsQueryResponse>(route);
    }

    public async Task<string> StartPlaylistAsync(string playlist)
    {
        if (string.IsNullOrWhiteSpace(playlist))
        {
            throw new ArgumentNullException("Invalid playlist name.");
        }

        string route = $"api/command/Start Playlist/{playlist}/true/false";
        return await _httpClient.GetAsync<string>(route);
    }
}