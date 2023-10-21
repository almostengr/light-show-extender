using Almostengr.Extensions;
using Almostengr.LightShowExtender.DomainService.Common;
using Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

namespace Almostengr.LightShowExtender.Infrastructure.FalconPiPlayer;

public sealed class FppHttpClient : IFppHttpClient
{
    private readonly AppSettings _appSettings;
    private readonly HttpClient _httpClient;

    public FppHttpClient(AppSettings appSettings, HttpClient httpClient)
    {
        _appSettings = appSettings;
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(_appSettings.FalconPlayer.ApiUrl.GetUrlWithProtocol());
    }

    public async Task<FppMediaMetaResponse> GetCurrentSongMetaDataAsync(string currentSong, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(currentSong))
        {
            throw new ArgumentNullException(nameof(currentSong));
        }

        string route = $"api/media/{currentSong}/meta";
        var response = await _httpClient.GetAsync(route, cancellationToken);
        await response.WasRequestSuccessfulAsync(cancellationToken);
        return await response.DeserializeResponseBodyAsync<FppMediaMetaResponse>(cancellationToken);
    }

    public async Task<FppStatusResponse> GetFppdStatusAsync(CancellationToken cancellationToken, string hostname = "")
    {
        string route = "api/fppd/status";

        if (!string.IsNullOrWhiteSpace(hostname))
        {
            hostname = hostname.GetUrlWithProtocol();
            route = $"{hostname}api/fppd/status";
        }

        var response = await _httpClient.GetAsync(route, cancellationToken);
        await response.WasRequestSuccessfulAsync(cancellationToken);
        return await response.DeserializeResponseBodyAsync<FppStatusResponse>(cancellationToken);
    }

    public async Task<FppMultiSyncSystemsResponse> GetMultiSyncSystemsAsync(CancellationToken cancellationToken)
    {
        string route = "api/fppd/multiSyncSystems";
        var response = await _httpClient.GetAsync(route, cancellationToken);
        await response.WasRequestSuccessfulAsync(cancellationToken);
        return await response.DeserializeResponseBodyAsync<FppMultiSyncSystemsResponse>(cancellationToken);
    }

    public async Task<string> GetInsertPlaylistAfterCurrent(string playlistName, CancellationToken cancellationToken)
    {
        string route = $"api/command/Insert Playlist After Current/{playlistName}";
        var response = await _httpClient.GetAsync(route, cancellationToken);
        await response.WasRequestSuccessfulAsync(cancellationToken);
        return await response.DeserializeResponseBodyAsync<string>(cancellationToken);
    }

    public async Task StopPlaylistGracefullyAsync(CancellationToken cancellationToken)
    {
        string route = "api/playlists/stopgracefully";
        var response = await _httpClient.GetAsync(route, cancellationToken);
        await response.WasRequestSuccessfulAsync(cancellationToken);
    }

    public async Task<List<string>> GetSequenceListAsync(CancellationToken cancellationToken)
    {
        string route = "api/sequence";
        var response = await _httpClient.GetAsync(route, cancellationToken);
        await response.WasRequestSuccessfulAsync(cancellationToken);
        return await response.DeserializeResponseBodyAsync<List<string>>(cancellationToken);
    }
}