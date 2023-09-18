using Almostengr.LightShowExtender.DomainService.Common;
using Almostengr.LightShowExtender.DomainService.FalconPiPlayer;
using Almostengr.LightShowExtender.DomainService.NwsWeather;
using Almostengr.LightShowExtender.DomainService.TheAlmostEngineer;

namespace Almostengr.LightShowExtender.DomainService.Monitoring;

public sealed class MonitoringService : BaseService, IMonitoringService
{
    private readonly IEngineerHttpClient _engineerHttpClient;
    private readonly IFppHttpClient _fppHttpClient;
    private readonly ILoggingService<MonitoringService> _logging;
    private readonly INwsHttpClient _nwsHttpClient;
    private readonly AppSettings _appSettings;
    private NwsLatestObservationResponseDto? _weatherObservation;
    private FppStatusResponseDto? _previousFppStatus;
    private FppStatusResponseDto? _currentFppStatus;

    public MonitoringService(IFppHttpClient fppHttpClient,
        IEngineerHttpClient engineerHttpClient,
        INwsHttpClient nwsHttpClient,
        AppSettings appSettings,
        ILoggingService<MonitoringService> logging)
    {
        _fppHttpClient = fppHttpClient;
        _engineerHttpClient = engineerHttpClient;
        _logging = logging;
        _nwsHttpClient = nwsHttpClient;
        _appSettings = appSettings;
    }

    public async Task<TimeSpan> MonitoringCheckAsync()
    {
        int delayTime = 5;
        try
        {
            if (_weatherObservation == null)
            {
                await GetLatestWeatherObservationsAsync();
            }

            _currentFppStatus = await _fppHttpClient.GetFppdStatusAsync();
            await StopPlaylistAfterEndTimeAsync();

            EngineerLightShowDisplayRequestDto displayDto = new();
            await GetAllPlayerCpuTemperaturesAsync(displayDto);
            displayDto.SetNwsTempC(_weatherObservation!.Properties.Temperature.Value.ToDisplayTemperature());
            displayDto.SetWindChill(_weatherObservation!.Properties.WindChill.Value.ToDisplayTemperature());

            await SetTitleAndArtist(displayDto);
            await _engineerHttpClient.PostDisplayInfoAsync(displayDto);

            _previousFppStatus = _currentFppStatus;
            const int MINUTES_15_IN_SECONDS = 900;
            delayTime = _currentFppStatus.Scheduler.CurrentPlaylist == null ?
                MINUTES_15_IN_SECONDS : Int32.Parse(_currentFppStatus.Seconds_Remaining + 1);
        }
        catch (Exception ex)
        {
            _logging.Error(ex, ex.Message);
        }

        return TimeSpan.FromSeconds(delayTime);
    }

    private async Task SetTitleAndArtist(EngineerLightShowDisplayRequestDto displayDto)
    {
        FppMediaMetaResponseDto currentSong =
            await _fppHttpClient.GetCurrentSongMetaDataAsync(_currentFppStatus!.Current_Song);

        if (currentSong != null)
        {
            string title = string.IsNullOrWhiteSpace(currentSong.Format.Tags.Title) ?
                GetSongNameFromFileName(_currentFppStatus.Current_Song) :
                currentSong.Format.Tags.Title;

            displayDto.SetTitle(title);
            displayDto.setArtist(currentSong.Format.Tags.Artist);
        }
    }

    public async Task GetLatestWeatherObservationsAsync()
    {
        try
        {
            _weatherObservation = await _nwsHttpClient.GetLatestObservation(_appSettings.NwsStationId);
        }
        catch (Exception ex)
        {
            _logging.Error(ex, ex.Message);
        }
    }

    private string GetSongNameFromFileName(string value)
    {
        value = Path.GetFileNameWithoutExtension(value)
            .Replace("_", " ")
            .Replace("-", " ");
        return value;
    }

    private async Task GetAllPlayerCpuTemperaturesAsync(EngineerLightShowDisplayRequestDto displayDto)
    {
        FppMultiSyncSystemsResponseDto fppMultiSyncSystemsDto = await _fppHttpClient.GetMultiSyncSystemsAsync();
        foreach (var system in fppMultiSyncSystemsDto.Systems)
        {
            var status = await _fppHttpClient.GetFppdStatusAsync(system.Address);

            float cpuTemperature = (float)status.Sensors
                .Where(s => s.Label.ToUpper().StartsWith("CPU"))
                .Select(s => s.Value)
                .Single();

            displayDto.AddCpuTemperature(cpuTemperature.ToDisplayTemperature());

            if (cpuTemperature >= _appSettings.FalconPlayer.MaxCpuTemperatureC)
            {
                _logging.Warning($"CPU temperature alert: {cpuTemperature.ToString()} for host {system.Hostname}");
            }
        }
    }

    private async Task StopPlaylistAfterEndTimeAsync()
    {
        TimeSpan currentTime = DateTime.Now.TimeOfDay;
        TimeSpan showEndTime = new TimeSpan(22, 15, 00);
        if (_currentFppStatus!.Scheduler.CurrentPlaylist.Playlist.ToLower().Contains("christmas") &&
            currentTime >= showEndTime)
        {
            _logging.Warning("Stopping playlist gracefully");
            await _fppHttpClient.StopPlaylistGracefully();
        }
    }

}