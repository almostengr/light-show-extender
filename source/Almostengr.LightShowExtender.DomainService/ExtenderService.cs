using System.Text;
using Almostengr.Common.NwsWeather;
using Almostengr.LightShowExtender.DomainService.Common;
using Almostengr.LightShowExtender.DomainService.FalconPiPlayer;
using Almostengr.LightShowExtender.DomainService.Wled;
using Almostengr.Common.TheAlmostEngineer;
using Almostengr.Common.Logging;
using Almostengr.Common.HomeAssistant;

namespace Almostengr.LightShowExtender.DomainService;

public sealed class ExtenderService : IExtenderService
{
    private readonly IEngineerHttpClient _engineerHttpClient;
    private readonly IFppHttpClient _fppHttpClient;
    private readonly IHomeAssistantHttpClient _haClient;
    private readonly ILoggingService<ExtenderService> _logging;
    private readonly INwsHttpClient _nwsHttpClient;
    private readonly IWledHttpClient _wledHttpClient;
    private readonly AppSettings _appSettings;
    private NwsLatestObservationResponseDto _weatherObservation;
    private DateTime _lastWeatherRefreshTime;
    private readonly TimeSpan _showEndTime;
    private string _previousSong;
    private uint _songsSincePsa;

    public ExtenderService(IFppHttpClient fppHttpClient,
        IEngineerHttpClient engineerHttpClient,
        IHomeAssistantHttpClient homeAssistantHttpClient,
        INwsHttpClient nwsHttpClient,
        IWledHttpClient wledHttpClient,
        AppSettings appSettings,
        ILoggingService<ExtenderService> logging)
    {
        _appSettings = appSettings;
        _fppHttpClient = fppHttpClient;
        _haClient = homeAssistantHttpClient;
        _engineerHttpClient = engineerHttpClient;
        _logging = logging;
        _nwsHttpClient = nwsHttpClient;
        _wledHttpClient = wledHttpClient;
        _lastWeatherRefreshTime = DateTime.Now.AddHours(-2);
        _showEndTime = new TimeSpan(22, 15, 00);
        _weatherObservation = new();
        _previousSong = "START";
        _songsSincePsa = 0;
    }

    public async Task<TimeSpan> MonitorAsync(CancellationToken cancellationToken)
    {
        const string DRIVEWAY_SWITCH = "switch.driveway";
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

                    // todo - turn off lights ran by WLED

                    TurnOffSwitchCommand turnOffCommand = new(DRIVEWAY_SWITCH);
                    TurnOffSwitchCommandHandler turnOffHandler = new(_haClient);
                    TurnOffSwitchResult turnOffResult = await turnOffHandler.HandleAsync(turnOffCommand, cancellationToken);
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

            EngineerDisplayRequestDto engineerDisplayRequestDto = await CreateDisplayRequestDtoAsync(currentStatus.Current_Song);
            await _engineerHttpClient.PostDisplayInfoAsync(engineerDisplayRequestDto);

            if (_songsSincePsa >= _appSettings.MaxSongsBetweenPsa)
            {
                List<string> allSequences = await _fppHttpClient.GetSequenceListAsync();
                string psaSequence = allSequences.Where(s => s.ToUpper().Contains("PSA")).First();
                psaSequence += ".fseq";
                EngineerResponseDto psaRequest = new EngineerResponseDto(psaSequence);
                await InsertFppPlaylistAsync(psaRequest.Message);
                _songsSincePsa = 0;
                _previousSong = currentStatus.Current_Song;
                return TimeSpan.FromSeconds(_appSettings.ExtenderDelay);
            }

            EngineerResponseDto unplayedRequest = await _engineerHttpClient.GetFirstUnplayedRequestAsync();
            if (unplayedRequest.Message.IsNullOrWhiteSpace())
            {
                return TimeSpan.FromSeconds(_appSettings.ExtenderDelay);
            }

            await InsertFppPlaylistAsync(unplayedRequest.Message);
            _songsSincePsa++;

            TurnOnSwitchCommand turnOnCommand = new(DRIVEWAY_SWITCH);
            TurnOnSwitchCommandHandler turnOnHandler = new(_haClient);
            TurnOnSwitchResult turnOnResult = await turnOnHandler.HandleAsync(turnOnCommand, cancellationToken);

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
        string cpuTemps = await CheckAllSystemsAsync();
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

    private async Task<string> CheckAllSystemsAsync()
    {
        FppMultiSyncSystemsResponseDto fppMultiSyncSystemsDto = await _fppHttpClient.GetMultiSyncSystemsAsync();
        StringBuilder output = new();

        foreach (var system in fppMultiSyncSystemsDto.Systems)
        {
            if (system.Type != FppSystemType.RaspberryPi3)
            {
                FppStatusResponseDto status = await _fppHttpClient.GetFppdStatusAsync(system.Address);

                float? cpuTemperature = (float)status.Sensors
                    .Where(s => s.Label.ToUpper().StartsWith("CPU"))
                    .Select(s => s.Value)
                    .SingleOrDefault();

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
            else if (system.Type == FppSystemType.WLED)
            {
                WledJsonResponseDto status = await _wledHttpClient.GetStatusAsync(system.Address);

                if (status.State.On == false)
                {
                    _logging.Warning($"WLED instance is not powered on {system.Hostname}");
                    WledJsonStateRequestDto wledRequestDto = new WledJsonStateRequestDto(true, 255);
                    WledJsonResponseDto response = await _wledHttpClient.PostStateAsync(system.Hostname, wledRequestDto);
                }
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
