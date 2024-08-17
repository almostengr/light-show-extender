using Almostengr.FalconPiPlayer.Common;
using Almostengr.FalconPiPlayer.DomainService.FalconPiPlayer;
using Almostengr.Extensions;

namespace Almostengr.FalconPiPlayer.Infrastructure.FalconPiPlayer;

public sealed class FppHttpClient : IFppHttpClient
{
    private readonly FppAppSettings _appSettings;
    private readonly HttpClient _httpClient;

    public FppHttpClient(FppAppSettings appSettings)
    {
        _appSettings = appSettings;
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(_appSettings.ApiUrl.GetUrlWithProtocol());
    }

    public async Task<MediaMetaResponse> GetCurrentSongMetaDataAsync(string currentSong, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(currentSong))
        {
            throw new ArgumentNullException(nameof(currentSong));
        }

        string route = $"api/media/{currentSong}/meta";
        return await _httpClient.GetAsync<MediaMetaResponse>(route, cancellationToken);
    }

    public async Task<FppStatusResponse> GetFppdStatusAsync(CancellationToken cancellationToken, string hostname = "")
    {
        string route = "api/fppd/status";

        if (!string.IsNullOrWhiteSpace(hostname))
        {
            hostname = hostname.GetUrlWithProtocol();
            route = $"{hostname}api/fppd/status";
        }

        return await _httpClient.GetAsync<FppStatusResponse>(route, cancellationToken);
    }

    public async Task<FppMultiSyncSystemsResponse> GetMultiSyncSystemsAsync(CancellationToken cancellationToken)
    {
        string route = "api/fppd/multiSyncSystems";
        return await _httpClient.GetAsync<FppMultiSyncSystemsResponse>(route, cancellationToken);
    }
}