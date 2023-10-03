using Almostengr.LightShowExtender.DomainService.Common;
using Almostengr.LightShowExtender.DomainService.FalconPiPlayer;
using Almostengr.LightShowExtender.Infrastructure.Common;

namespace Almostengr.LightShowExtender.Infrastructure.FalconPiPlayer;

public sealed class FppHttpClient : BaseHttpClient, IFppHttpClient
{
    private readonly AppSettings _appSettings;
    private readonly HttpClient _httpClient;

    public FppHttpClient(AppSettings appSettings)
    {
        _appSettings = appSettings;
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(GetUrlWithProtocol(_appSettings.FalconPlayer.ApiUrl));
    }

    public async Task<FppMediaMetaResponseDto> GetCurrentSongMetaDataAsync(string currentSong)
    {
        if (string.IsNullOrWhiteSpace(currentSong))
        {
            throw new ArgumentNullException(nameof(currentSong));
        }

        string route = $"api/media/{currentSong}/meta";
        return await HttpGetAsync<FppMediaMetaResponseDto>(_httpClient, route);
    }

    public async Task<FppStatusResponseDto> GetFppdStatusAsync()
    {
        string route = "api/fppd/status";
        return await HttpGetAsync<FppStatusResponseDto>(_httpClient, route);
    }

    public async Task<FppStatusResponseDto> GetFppdStatusAsync(string hostname)
    {
        hostname = GetUrlWithProtocol(hostname);
        string route = $"{hostname}api/fppd/status";
        return await HttpGetAsync<FppStatusResponseDto>(_httpClient, route);
    }

    public async Task<FppMultiSyncSystemsResponseDto> GetMultiSyncSystemsAsync()
    {
        string route = "api/fppd/multiSyncSystems";
        return await HttpGetAsync<FppMultiSyncSystemsResponseDto>(_httpClient, route);
    }

    public async Task<string> GetInsertPlaylistAfterCurrent(string playlistName)
    {
        string route = $"api/command/Insert Playlist After Current/{playlistName}";
        return await HttpGetAsync<string>(_httpClient, route);
    }

    public async Task StopPlaylistGracefullyAsync()
    {
        string route = "api/playlists/stopgracefully";
        await HttpGetAsync<string>(_httpClient, route);
    }

    public async Task<List<string>> GetSequenceListAsync()
    {
        string route = "api/sequence";
        return await HttpGetAsync<List<string>>(_httpClient, route);
    }
}