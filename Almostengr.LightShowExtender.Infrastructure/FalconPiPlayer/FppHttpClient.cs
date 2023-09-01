using Almostengr.LightShowExtender.DomainService.Common;
using Almostengr.LightShowExtender.DomainService.Common.Constants;
using Almostengr.LightShowExtender.DomainService.FalconPiPlayer;
using Almostengr.LightShowExtender.Infrastructure.Common;

namespace Almostengr.LightShowExtender.Infrastructure.FalconPiPlayer;

internal sealed class FppHttpClient : BaseClient, IFppHttpClient
{
    private readonly ILoggingService<FppHttpClient> _logger;
    private readonly HttpClient _httpClient;

    public FppHttpClient(ILoggingService<FppHttpClient> logger)
    {
        _logger = logger;
        _httpClient = new HttpClient();
    }

    public async Task<FppMediaMetaDto> GetCurrentSongMetaDataAsync(string currentSong)
    {
        _logger.Debug("Getting current song meta data");

        if (string.IsNullOrWhiteSpace(currentSong))
        {
            _logger.Warning("No song provided");
            return null;
        }

        string hostname = GetUrlWithProtocol(AppConstants.Localhost);
        return await HttpGetAsync<FppMediaMetaDto>(_httpClient, $"{hostname}api/media/{currentSong}/meta");
    }

    public async Task<FppStatusDto> GetFppdStatusAsync(string address)
    {
        string hostname = GetUrlWithProtocol(address);
        return await HttpGetAsync<FppStatusDto>(_httpClient, $"{hostname}api/fppd/status");
    }

    public async Task GetInsertPlaylistAfterCurrent(string playlistName)
    {
        string hostname = GetUrlWithProtocol(AppConstants.Localhost);
        string route = $"{hostname}/Insert Playlist After Current/${playlistName}";
        await HttpGetAsync<FppCommandDto>(_httpClient, route);
    }
}