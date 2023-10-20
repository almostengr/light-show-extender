using Almostengr.Common.Logging;
using Almostengr.LightShowExtender.DomainService.Common;

namespace Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

public sealed class FppService : IFppService
{
    private readonly IFppHttpClient _httpClient;
    private readonly AppSettings _appSettings;
    private readonly ILoggingService<FppService> _loggingService;

    public FppService(AppSettings appSettings, IFppHttpClient httpClient, ILoggingService<FppService> loggingService)
    {
        _httpClient = httpClient;
        _appSettings = appSettings;
        _loggingService = loggingService;
    }

    public async Task<FppMediaMetaResponse> GetCurrentSongMetaDataAsync(string currentSong, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(currentSong))
        {
            throw new ArgumentNullException(nameof(currentSong));
        }

        return await _httpClient.GetCurrentSongMetaDataAsync(currentSong, cancellationToken);
    }

    public async Task<FppStatusResponse> GetFppdStatusAsync(CancellationToken cancellationToken, string hostname = "")
    {
        return await _httpClient.GetFppdStatusAsync(cancellationToken, hostname);
    }

    public async Task InsertPlaylistAfterCurrentAsync(string playlistName, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(playlistName))
        {
            throw new ArgumentNullException(nameof(playlistName));
        }

        var response =  await _httpClient.GetInsertPlaylistAfterCurrent(playlistName, cancellationToken);

        if (response.ToUpper() != "PLAYLIST INSERTED")
        {
            throw new InvalidDataException($"Unexpected response from FPP. {response}");
        }
    }

    public async Task<FppMultiSyncSystemsResponse> GetMultiSyncSystemsAsync(CancellationToken cancellationToken)
    {
        return await _httpClient.GetMultiSyncSystemsAsync(cancellationToken);
    }

    public async Task<List<string>> GetSequenceListAsync(CancellationToken cancellationToken)
    {
        return await _httpClient.GetSequenceListAsync(cancellationToken);
    }

    public async Task StopPlaylistGracefullyAsync(CancellationToken cancellationToken)
    {
        await _httpClient.StopPlaylistGracefullyAsync(cancellationToken);
    }

    public async Task StopPlaylistAfterEndTimeAsync(string currentPlaylist, CancellationToken cancellationToken)
    {
        var showEndTime = new TimeSpan(22, 15, 00);
        if (currentPlaylist.ToUpper().Contains("CHRISTMAS") && DateTime.Now.TimeOfDay >= showEndTime)
        {
            _loggingService.Warning("Stopping playlist gracefully");
            await _httpClient.StopPlaylistGracefullyAsync(cancellationToken);
        }
    }


    public string GetSongNameFromFileName(string value)
    {
        value = Path.GetFileNameWithoutExtension(value)
            .Replace("_", " ")
            .Replace("-", " ")
            .Replace("  ", " ");
        return value;
    }
}

public interface IFppService
{
    Task<FppMediaMetaResponse> GetCurrentSongMetaDataAsync(string currentSong, CancellationToken cancellationToken);
    Task<FppStatusResponse> GetFppdStatusAsync(CancellationToken cancellationToken, string hostname = "");
    Task InsertPlaylistAfterCurrentAsync(string playlistName, CancellationToken cancellationToken);
    Task<FppMultiSyncSystemsResponse> GetMultiSyncSystemsAsync(CancellationToken cancellationToken);
    Task<List<string>> GetSequenceListAsync(CancellationToken cancellationToken);
    Task StopPlaylistAfterEndTimeAsync(string currentPlaylist, CancellationToken cancellationToken);
    Task StopPlaylistGracefullyAsync(CancellationToken cancellationToken);
    string GetSongNameFromFileName(string value);
}