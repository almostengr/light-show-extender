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
        _previousSong = string.Empty;
    }

    public async Task<TimeSpan> UpdateWebsiteDisplayAsync()
    {
        const uint DELAY_DURATION = 10;
        const uint MIN_SECONDS_REMAINING = 5;
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
                return TimeSpan.FromSeconds(DELAY_DURATION);
            }
        }
        catch (Exception ex)
        {
            _logging.Error(ex, ex.GetBaseException().ToString());
            return TimeSpan.FromSeconds(DELAY_DURATION);
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

            if (currentStatus.Current_Song == _previousSong || currentStatus.SecondsRemaining() > MIN_SECONDS_REMAINING)
            {
                _previousSong = currentStatus.Current_Song;
                return TimeSpan.FromSeconds(DELAY_DURATION);
            }

            EngineerDisplayRequestDto engineerDisplayRequestDto = await CreateDisplayRequestDtoAsync(currentStatus.Current_Song);
            await _engineerHttpClient.PostDisplayInfoAsync(engineerDisplayRequestDto);

            if (currentStatus.Current_Song.ToUpper().StartsWith("HPL"))
            {
                return TimeSpan.FromSeconds(currentStatus.SecondsRemaining() - 2);
            }

            EngineerResponseDto unplayedRequest = await _engineerHttpClient.GetFirstUnplayedRequestAsync();
            if (unplayedRequest.Message.IsNullOrWhiteSpace())
            {
                unplayedRequest = await GetRandomSequenceAsync();
            }

            await InsertFppPlaylistAsync(unplayedRequest.Message);

            _previousSong = currentStatus.Current_Song;
        }
        catch (Exception ex)
        {
            _logging.Error(ex, ex.GetBaseException().ToString());
        }

        return TimeSpan.FromSeconds(DELAY_DURATION);
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

        _logging.Information(engineerDisplayRequestDto.ToString());
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
        DateTime oneHourAgo = DateTime.Now.AddHours(-1);

        if (_lastWeatherRefreshTime > oneHourAgo)
        {
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
            var status = await _fppHttpClient.GetFppdStatusAsync(system.Address);

            float cpuTemperature = (float)status.Sensors
                .Where(s => s.Label.ToUpper().StartsWith("CPU"))
                .Select(s => s.Value)
                .Single();

            if (output.Length > 0)
            {
                output.Append(", ");
            }

            output.Append($"{cpuTemperature.ToDisplayTemperature()} ");

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

