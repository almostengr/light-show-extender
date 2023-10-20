using Almostengr.Common.Utilities;
using Almostengr.LightShowExtender.DomainService.Common;
using Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

namespace Almostengr.LightShowExtender.Infrastructure.FalconPiPlayer;

public sealed class FppHttpClient : BaseHttpClient, IFppHttpClient
{
    private readonly AppSettings _appSettings;

    public FppHttpClient(AppSettings appSettings)
    {
        _appSettings = appSettings;
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(GetUrlWithProtocol(_appSettings.FalconPlayer.ApiUrl));
    }

    public async Task<FppMediaMetaResponse> GetCurrentSongMetaDataAsync(string currentSong, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(currentSong))
        {
            throw new ArgumentNullException(nameof(currentSong));
        }

        string route = $"api/media/{currentSong}/meta";
        return await HttpGetAsync<FppMediaMetaResponse>(route, cancellationToken);
    }

    public async Task<FppStatusResponse> GetFppdStatusAsync(CancellationToken cancellationToken, string hostname = "")
    {
        string route = "api/fppd/status";

        if (!string.IsNullOrWhiteSpace(hostname))
        {
            hostname = GetUrlWithProtocol(hostname);
            route = $"{hostname}api/fppd/status";
        }

        return await HttpGetAsync<FppStatusResponse>(route, cancellationToken);
    }

    public async Task<FppMultiSyncSystemsResponse> GetMultiSyncSystemsAsync(CancellationToken cancellationToken)
    {
        string route = "api/fppd/multiSyncSystems";
        return await HttpGetAsync<FppMultiSyncSystemsResponse>(route, cancellationToken);
    }

    public async Task<string> GetInsertPlaylistAfterCurrent(string playlistName, CancellationToken cancellationToken)
    {
        string route = $"api/command/Insert Playlist After Current/{playlistName}";
        return await HttpGetAsync<string>(route, cancellationToken);
    }

    public async Task StopPlaylistGracefullyAsync(CancellationToken cancellationToken)
    {
        string route = "api/playlists/stopgracefully";
        await HttpGetAsync<string>(route, cancellationToken);
    }

    public async Task<List<string>> GetSequenceListAsync(CancellationToken cancellationToken)
    {
        string route = "api/sequence";
        return await HttpGetAsync<List<string>>(route, cancellationToken);
    }
}