using Almostengr.LightShowExtender.DomainService.Common;
using Almostengr.LightShowExtender.DomainService.FalconPiPlayer;
using Almostengr.LightShowExtender.DomainService.NwsWeather;

namespace Almostengr.LightShowExtender.DomainService.TheAlmostEngineer;

public sealed class DisplayService : BaseService, IDisplayService
{
    private readonly IEngineerHttpClient _engineerHttpClient;
    private readonly IFppHttpClient _fppHttpClient;
    private readonly ILoggingService<DisplayService> _logging;
    private readonly INwsHttpClient _nwsHttpClient;
    private readonly AppSettings _appSettings;
    private NwsLatestObservationResponseDto? _weatherObservation;
    private FppStatusResponseDto _previousStatus;
    private DateTime _lastWeatherRefreshTime;
    private readonly TimeSpan _showEndTime;

    public DisplayService(IFppHttpClient fppHttpClient,
        IEngineerHttpClient engineerHttpClient,
        INwsHttpClient nwsHttpClient,
        AppSettings appSettings,
        ILoggingService<DisplayService> logging)
    {
        _fppHttpClient = fppHttpClient;
        _engineerHttpClient = engineerHttpClient;
        _logging = logging;
        _nwsHttpClient = nwsHttpClient;
        _appSettings = appSettings;
        _previousStatus = new FppStatusResponseDto();
        _lastWeatherRefreshTime = DateTime.Now.AddHours(-2);
        _showEndTime = new TimeSpan(22, 15, 00);
    }


    public async Task<TimeSpan> UpdateWebsiteDisplayAsync()
    {
        const int DELAY_DURATION = 10;

        try
        {
            /*
            check the weather
            check fpp

            if the weather has been updated or the song has changed, then post update to website
            */
            bool wasWeatherRefreshed = await GetWeatherObservationAsync();
            FppStatusResponseDto currentStatus = await _fppHttpClient.GetFppdStatusAsync();
            await StopPlaylistAfterEndTimeAsync(currentStatus.Scheduler.CurrentPlaylist.Playlist);

            bool didSongChange = _previousStatus.Current_Song.IsSameSong(currentStatus.Current_Song);
            
            if (!didSongChange && !wasWeatherRefreshed)
            {
                return TimeSpan.FromSeconds(DELAY_DURATION);
            }

            EngineerDisplayRequestDto displayDto = new();
            await GetAllCpuTemperaturesAsync(displayDto);
            displayDto.SetNwsTempC(_weatherObservation!.Properties.Temperature.Value.ToDisplayTemperature());
            displayDto.SetWindChill(_weatherObservation!.Properties.WindChill.Value.ToDisplayTemperature());
            await SetTitleAndArtistAsync(displayDto, currentStatus.Current_Song);
            _logging.Information(displayDto.ToString());

            await _engineerHttpClient.PostDisplayInfoAsync(displayDto);
            _previousStatus = currentStatus;
        }
        catch (Exception ex)
        {
            _logging.Error(ex, ex.Message);
        }

        return TimeSpan.FromSeconds(DELAY_DURATION);
    }

    private async Task SetTitleAndArtistAsync(EngineerDisplayRequestDto displayDto, string currentSong)
    {
        if (string.IsNullOrWhiteSpace(currentSong))
        {
            return;
        }

        FppMediaMetaResponseDto metaResponse = await _fppHttpClient.GetCurrentSongMetaDataAsync(currentSong);

        if (metaResponse != null)
        {
            string title = string.IsNullOrWhiteSpace(metaResponse.Format.Tags.Title) ?
                GetSongNameFromFileName(currentSong) :
                metaResponse.Format.Tags.Title;

            displayDto.SetTitle(title);
            displayDto.setArtist(metaResponse.Format.Tags.Artist);
        }
    }

    private async Task<bool> GetWeatherObservationAsync()
    {
        try
        {
            DateTime oneHourAgo = DateTime.Now.AddHours(-1);

            if (_lastWeatherRefreshTime >= oneHourAgo)
            {
                _weatherObservation = await _nwsHttpClient.GetLatestObservationAsync(_appSettings.NwsStationId);
                _lastWeatherRefreshTime = DateTime.Now;
                return true;
            }
        }
        catch (Exception ex)
        {
            _logging.Error(ex, ex.Message);
        }

        return false;
    }

    private string GetSongNameFromFileName(string value)
    {
        value = Path.GetFileNameWithoutExtension(value).Replace("_", " ").Replace("-", " ");
        return value;
    }

    private async Task GetAllCpuTemperaturesAsync(EngineerDisplayRequestDto displayDto)
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

    private async Task StopPlaylistAfterEndTimeAsync(string currentPlaylist)
    {
        if (currentPlaylist.Contains("christmas") && DateTime.Now.TimeOfDay >= _showEndTime)
        {
            _logging.Warning("Stopping playlist gracefully");
            await _fppHttpClient.StopPlaylistGracefullyAsync();
        }
    }

}