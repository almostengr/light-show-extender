using System.Text;
using Almostengr.LightShowExtender.DomainService.Common;
using Almostengr.LightShowExtender.DomainService.FalconPiPlayer;
using Almostengr.LightShowExtender.DomainService.NwsWeather;
using Almostengr.LightShowExtender.DomainService.TheAlmostEngineer;

namespace Almostengr.LightShowExtender.DomainService;

public sealed class ExtenderService : IExtenderService
{
    private readonly IEngineerHttpClient _engineerHttpClient;
    private readonly IFppHttpClient _fppHttpClient;
    private readonly ILoggingService<ExtenderService> _logging;
    private readonly INwsHttpClient _nwsHttpClient;
    private readonly AppSettings _appSettings;
    private NwsLatestObservationResponseDto _weatherObservation;
    private DateTime _lastWeatherRefreshTime;
    private readonly TimeSpan _showEndTime;
    private string _previousSong;
    private uint _songsSincePsa;

    public ExtenderService(IFppHttpClient fppHttpClient,
        IEngineerHttpClient engineerHttpClient,
        INwsHttpClient nwsHttpClient,
        AppSettings appSettings,
        ILoggingService<ExtenderService> logging)
    {
        _fppHttpClient = fppHttpClient;
        _engineerHttpClient = engineerHttpClient;
        _logging = logging;
        _nwsHttpClient = nwsHttpClient;
        _appSettings = appSettings;
        _lastWeatherRefreshTime = DateTime.Now.AddHours(-2);
        _showEndTime = new TimeSpan(22, 15, 00);
        _weatherObservation = new();
        _previousSong = "START";
        _songsSincePsa = 0;
    }

    public async Task<TimeSpan> UpdateWebsiteDisplayAsync()
    {
        FppStatusResponseDto currentStatus;

        try
        {
            currentStatus = await _fppHttpClient.GetFppdStatusAsync();

            if (currentStatus.Current_Song.IsNullOrWhiteSpace())
            {
                if (_previousSong.IsNotNullOrWhiteSpace())
                {
                    EngineerDisplayRequestDto displayRequestDto = await CreateDisplayRequestDtoAsync(string.Empty);
                    await _engineerHttpClient.PostDisplayInfoAsync(displayRequestDto);
                }

                _previousSong = currentStatus.Current_Song;
                return TimeSpan.FromSeconds(_appSettings.ExtenderDelay);
            }
        }
        catch (Exception ex)
        {
            _logging.Error(ex, ex.GetBaseException().ToString());
            return TimeSpan.FromSeconds(_appSettings.ExtenderDelay);
        }

        try
        {
            await GetWeatherObservationAsync();
        }
        catch (Exception ex)
        {
            _logging.Error(ex, ex.GetBaseException().ToString());
        }

        try
        {
            await ClearQueueWhenStartingPlaylistAsync(currentStatus);
            await StopPlaylistAfterEndTimeAsync(currentStatus.Scheduler.CurrentPlaylist.Playlist);

            if (currentStatus.Current_Song == _previousSong)
            {
                _previousSong = currentStatus.Current_Song;
                return TimeSpan.FromSeconds(_appSettings.ExtenderDelay);
            }

            _logging.Information($"Previous song: {_previousSong}; current song: {currentStatus.Current_Song}");

            EngineerDisplayRequestDto engineerDisplayRequestDto = await CreateDisplayRequestDtoAsync(currentStatus.Current_Song);
            await _engineerHttpClient.PostDisplayInfoAsync(engineerDisplayRequestDto);

            if (_songsSincePsa >= _appSettings.MaxSongsBetweenPsa)
            {
                List<string> allSequences = await _fppHttpClient.GetSequenceListAsync();
                string psaSequence = allSequences.Where(s => s.ToUpper().Contains("PSA")).First();
                psaSequence += ".fseq";
                EngineerResponseDto psaRequest = new EngineerResponseDto { Message = psaSequence };
                await InsertFppPlaylistAsync(psaRequest.Message);
                _songsSincePsa = 0;
                _previousSong = currentStatus.Current_Song;
                return TimeSpan.FromSeconds(_appSettings.ExtenderDelay);
            }

            EngineerResponseDto unplayedRequest = await _engineerHttpClient.GetFirstUnplayedRequestAsync();
            if (unplayedRequest.Message.IsNullOrWhiteSpace())
            {
                unplayedRequest = await GetRandomSequenceAsync();
            }

            await InsertFppPlaylistAsync(unplayedRequest.Message);
            _songsSincePsa++;

            _previousSong = currentStatus.Current_Song;
        }
        catch (Exception ex)
        {
            _logging.Error(ex, ex.GetBaseException().ToString());
        }

        return TimeSpan.FromSeconds(_appSettings.ExtenderDelay);
    }

    private async Task<EngineerDisplayRequestDto> CreateDisplayRequestDtoAsync(string currentSong)
    {
        string cpuTemps = await GetAllCpuTemperaturesAsync();
        string windChill = _weatherObservation.Properties.WindChill.Value.ToDisplayTemperature();
        string temp = _weatherObservation.Properties.Temperature.Value.ToDisplayTemperature();
        string artist = string.Empty;
        string title = string.Empty;

        if (currentSong.IsNotNullOrWhiteSpace())
        {
            FppMediaMetaResponseDto metaResponse = await _fppHttpClient.GetCurrentSongMetaDataAsync(currentSong);

            if (metaResponse != null)
            {
                title = metaResponse.Format.Tags.Title.IsNullOrWhiteSpace() ?
                    GetSongNameFromFileName(currentSong) : metaResponse.Format.Tags.Title;

                artist = metaResponse.Format.Tags.Artist.IsNullOrWhiteSpace() ?
                   string.Empty : metaResponse.Format.Tags.Artist;
            }
        }

        EngineerDisplayRequestDto engineerDisplayRequestDto = new EngineerDisplayRequestDto
        {
            CpuTemp = cpuTemps,
            WindChill = windChill,
            NwsTemperature = temp,
            Title = title,
            Artist = artist,
        };

        return engineerDisplayRequestDto;
    }

    private async Task ClearQueueWhenStartingPlaylistAsync(FppStatusResponseDto currentStatus)
    {
        if (_previousSong.IsNullOrWhiteSpace() && currentStatus.Current_Song.IsNotNullOrWhiteSpace())
        {
            await _engineerHttpClient.DeleteAllSongsInQueueAsync();
        }
    }

    private async Task<EngineerResponseDto> GetRandomSequenceAsync()
    {
        List<string> allSequences = await _fppHttpClient.GetSequenceListAsync();
        List<string> filteredSequences = allSequences.Where(s => !s.ToUpper().StartsWith("HPL")).ToList();

        Random random = new();
        string selectedSequence = filteredSequences.ElementAt(random.Next(filteredSequences.Count()));
        selectedSequence += ".fseq";
        EngineerResponseDto responseDto = new(selectedSequence);
        return responseDto;
    }

    private async Task InsertFppPlaylistAsync(string sequenceFileName)
    {
        string fppResponse = await _fppHttpClient.GetInsertPlaylistAfterCurrent(sequenceFileName);
        if (fppResponse.ToUpper() != "PLAYLIST INSERTED")
        {
            throw new InvalidDataException($"Unexpected response from FPP. {fppResponse}");
        }
    }

    private async Task GetWeatherObservationAsync()
    {
        DateTime nextRefreshTime = DateTime.Now.AddHours(-1);

        if (_lastWeatherRefreshTime < nextRefreshTime)
        {
            _logging.Information("Refreshing weather information");
            _weatherObservation = await _nwsHttpClient.GetLatestObservationAsync(_appSettings.NwsStationId);
            _lastWeatherRefreshTime = DateTime.Now;
        }
    }

    private string GetSongNameFromFileName(string value)
    {
        value = Path.GetFileNameWithoutExtension(value)
            .Replace("_", " ")
            .Replace("-", " ")
            .Replace("  ", " ");
        return value;
    }

    private async Task<string> GetAllCpuTemperaturesAsync()
    {
        FppMultiSyncSystemsResponseDto fppMultiSyncSystemsDto = await _fppHttpClient.GetMultiSyncSystemsAsync();
        StringBuilder output = new();

        foreach (var system in fppMultiSyncSystemsDto.Systems)
        {
            FppStatusResponseDto status = await _fppHttpClient.GetFppdStatusAsync(system.Address);

            float cpuTemperature = (float)status.Sensors
                .Where(s => s.Label.ToUpper().StartsWith("CPU"))
                .Select(s => s.Value)
                .Single();

            if (output.Length > 0)
            {
                output.Append(", ");
            }

            output.Append(cpuTemperature.ToDisplayTemperature());

            if (cpuTemperature >= _appSettings.FalconPlayer.MaxCpuTemperatureC)
            {
                _logging.Warning($"CPU temperature alert: {cpuTemperature.ToString()} for host {system.Hostname}");
            }
        }

        return output.ToString();
    }

    private async Task StopPlaylistAfterEndTimeAsync(string currentPlaylist)
    {
        if (currentPlaylist.ToUpper().Contains("CHRISTMAS") && DateTime.Now.TimeOfDay >= _showEndTime)
        {
            _logging.Warning("Stopping playlist gracefully");
            await _fppHttpClient.StopPlaylistGracefullyAsync();
        }
    }

}
