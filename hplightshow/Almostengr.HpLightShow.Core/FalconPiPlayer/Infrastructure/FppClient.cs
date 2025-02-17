using Almostengr.Common.Infrastructure;
using Almostengr.HpLightShow.Core.FalconPiPlayer.DomainServices;
using Almostengr.HpLightShow.Core.FalconPiPlayer.DomainServices.Infrastructure;
using Almostengr.HpLightShow.Core.FalconPiPlayer.Shared;

namespace Almostengr.HpLightShow.Core.FalconPiPlayer.Infrastructure;

public sealed class FppClient : IFppClient
{
    private readonly FppAppSettings _appSettings;
    private readonly HttpClient _httpClient;

    public FppClient(HttpClient httpClient, FppAppSettings appSettings)
    {
        _appSettings = appSettings;

        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(_appSettings.FppApiUrl);
        _httpClient.Timeout = TimeSpan.FromSeconds(15);
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    public async Task<FppStatusResource> GetFppdStatusAsync()
    {
        string route = "api/fppd/status";
        return await _httpClient.GetAsync<FppStatusResource>(route);
    }

    public async Task<string> StartPlaylistAsync(string playlist)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(playlist, nameof(playlist));

        string route = $"api/command/Start Playlist/{playlist}/true/false";
        return await _httpClient.GetStringAsync(route);
    }

    public async Task<MultiSyncSystemsResource> MultiSyncSystemsResource()
    {
        string route = "api/fppd/multiSyncSystems";
        return await _httpClient.GetAsync<MultiSyncSystemsResource>(route);
    }
}