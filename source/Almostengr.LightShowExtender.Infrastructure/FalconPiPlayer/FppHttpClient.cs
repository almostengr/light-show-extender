using Almostengr.LightShowExtender.DomainService.Common;
using Almostengr.LightShowExtender.DomainService.FalconPiPlayer;
using Almostengr.LightShowExtender.Infrastructure.Common;

namespace Almostengr.LightShowExtender.Infrastructure.FalconPiPlayer;

public sealed class FppHttpClient : BaseHttpClient, IFppHttpClient
{
    private readonly AppSettings _appSettings;
    private readonly ILoggingService<FppHttpClient> _logger;
    private readonly HttpClient _httpClient;

    public FppHttpClient(ILoggingService<FppHttpClient> logger, AppSettings appSettings)
    {
        _appSettings = appSettings;
        _logger = logger;
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(GetUrlWithProtocol(_appSettings.FalconPlayer.ApiUrl));
    }

    public async Task<FppMediaMetaDto> GetCurrentSongMetaDataAsync(string currentSong)
    {
        _logger.Debug("Getting current song meta data");

        if (string.IsNullOrWhiteSpace(currentSong))
        {
            throw new ArgumentNullException(nameof(currentSong));
        }

        string route = $"api/media/{currentSong}/meta";
        return await HttpGetAsync<FppMediaMetaDto>(_httpClient, route);
    }

    public async Task<FppStatusDto> GetFppdStatusAsync()
    {
        string route = "api/fppd/status";
        return await HttpGetAsync<FppStatusDto>(_httpClient, route);
    }

    public async Task GetInsertPlaylistAfterCurrent(string playlistName)
    {
        string route = $"api/command/Insert Playlist After Current/${playlistName}";
        await HttpGetAsync<FppCommandDto>(_httpClient, route);
    }
}