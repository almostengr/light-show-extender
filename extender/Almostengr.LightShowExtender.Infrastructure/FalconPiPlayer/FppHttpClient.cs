using Almostengr.Extensions;
using Almostengr.LightShowExtender.DomainService.Common;
using Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

namespace Almostengr.LightShowExtender.Infrastructure.FalconPiPlayer;

public sealed class FppHttpClient : IFppHttpClient
{
    private readonly AppSettings _appSettings;
    private readonly HttpClient _httpClient;

    public FppHttpClient(AppSettings appSettings)
    {
        _appSettings = appSettings;
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(_appSettings.FalconPlayer.ApiUrl.GetUrlWithProtocol());
    }

    public async Task<FppMediaMetaResponse> GetCurrentSongMetaDataAsync(string currentSong, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(currentSong))
        {
            throw new ArgumentNullException(nameof(currentSong));
        }

        string route = $"api/media/{currentSong}/meta";
        return await _httpClient.GetAsync<FppMediaMetaResponse>(route, cancellationToken);
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

    public async Task<string> GetInsertPlaylistAfterCurrent(string playlistName, CancellationToken cancellationToken)
    {
        string route = $"api/command/Insert Playlist After Current/{playlistName}";
        return await _httpClient.GetStringAsync(route, cancellationToken);
    }

    public async Task StopPlaylistGracefullyAsync(CancellationToken cancellationToken)
    {
        string route = "api/playlists/stopgracefully";
        await _httpClient.GetAsync(route, cancellationToken);
    }
}