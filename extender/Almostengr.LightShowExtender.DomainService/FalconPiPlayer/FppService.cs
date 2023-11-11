using System.Text;
using Almostengr.Common.Logging;
using Almostengr.LightShowExtender.DomainService.Common;

namespace Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

public sealed class FppService : IFppService
{
    private readonly IFppHttpClient _httpClient;
    private readonly AppSettings _appSettings;
    private readonly ILoggingService<FppService> _loggingService;
    private FppMultiSyncSystemsResponse _multiSyncSystems;
    private DateTime _lastMultiSyncTime;

    public FppService(AppSettings appSettings, IFppHttpClient httpClient, ILoggingService<FppService> loggingService)
    {
        _httpClient = httpClient;
        _appSettings = appSettings;
        _loggingService = loggingService;
        _lastMultiSyncTime = DateTime.Now.AddHours(-2);
        _multiSyncSystems = new();
    }

    public async Task<FppMediaMetaResponse> GetCurrentSongMetaDataAsync(string currentSong, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(currentSong))
        {
            return new();
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

        var response = await _httpClient.GetInsertPlaylistAfterCurrent(playlistName, cancellationToken);

        if (response.ToUpper() != "PLAYLIST INSERTED")
        {
            throw new InvalidDataException($"Unexpected response from FPP. {response}");
        }
    }

    public async Task<FppMultiSyncSystemsResponse> GetMultiSyncSystemsAsync(CancellationToken cancellationToken, bool forceRefresh = false)
    {
        TimeSpan timeDifference = DateTime.Now - _lastMultiSyncTime;
        if (forceRefresh || timeDifference.Hours >= 1)
        {
            _multiSyncSystems = await _httpClient.GetMultiSyncSystemsAsync(cancellationToken);
            _lastMultiSyncTime = DateTime.Now;
        }

        return _multiSyncSystems;
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

    public async Task<List<string>> GetWledSystemsFromMultiSyncSystemsAsync(CancellationToken cancellationToken, bool forceRefresh = false)
    {
        var multiSyncSystems = await GetMultiSyncSystemsAsync(cancellationToken, forceRefresh);
        return multiSyncSystems.Systems.Where(s => s.Type == "WLED")
            .Select(s => s.Address)
            .ToList();
    }

    public async Task<string> GetCpuTemperaturesAsync(CancellationToken cancellationToken)
    {
        var multiSyncSystems = await GetMultiSyncSystemsAsync(cancellationToken, false);

        var fppSystems = multiSyncSystems.Systems.Where(s => s.Type.StartsWith("Raspberry Pi"))
            .Select(s => s.Address)
            .ToList();

        StringBuilder output = new();
        foreach (var system in fppSystems)
        {
            var response = await GetFppdStatusAsync(cancellationToken, system);
            var temp = (float)response.Sensors.Where(s => s.Label.StartsWith("CPU"))
                .Select(s => s.Value)
                .Single();

            if (output.Length > 0)
            {
                output.Append(", ");
            }

            output.Append(temp.ToDisplayTemperature());
        }

        return output.ToString();
    }

    public string GetSongNameFromFileName(string value)
    {
        value = Path.GetFileNameWithoutExtension(value)
            .Replace("_", " ")
            .Replace("-", " ")
            .Replace("  ", " ");
        return value;
    }

    public async Task InsertPsaAsync(CancellationToken cancellationToken)
    {
        List<string> allSequences = await GetSequenceListAsync(cancellationToken);
        string psaSequence = allSequences.Where(s => s.ToUpper().Contains("PSA")).First();
        psaSequence = psaSequence.Contains(".fseq") ? psaSequence : $"{psaSequence}.fseq";
        await InsertPlaylistAfterCurrentAsync(psaSequence, cancellationToken);
    }
}

public interface IFppService
{
    Task<FppMediaMetaResponse> GetCurrentSongMetaDataAsync(string currentSong, CancellationToken cancellationToken);
    Task<FppStatusResponse> GetFppdStatusAsync(CancellationToken cancellationToken, string hostname = "");
    Task InsertPlaylistAfterCurrentAsync(string playlistName, CancellationToken cancellationToken);
    Task<List<string>> GetSequenceListAsync(CancellationToken cancellationToken);
    Task StopPlaylistAfterEndTimeAsync(string currentPlaylist, CancellationToken cancellationToken);
    Task StopPlaylistGracefullyAsync(CancellationToken cancellationToken);
    string GetSongNameFromFileName(string value);
    Task<FppMultiSyncSystemsResponse> GetMultiSyncSystemsAsync(CancellationToken cancellationToken, bool forceRefresh = false);
    Task<List<string>> GetWledSystemsFromMultiSyncSystemsAsync(CancellationToken cancellationToken, bool forceRefresh = false);
    Task<string> GetCpuTemperaturesAsync(CancellationToken cancellationToken);
    Task InsertPsaAsync(CancellationToken cancellationToken);
}