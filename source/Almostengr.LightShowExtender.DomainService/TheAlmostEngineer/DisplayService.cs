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
    private NwsLatestObservationResponseDto _weatherObservation;
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
        _previousStatus = new();
        _lastWeatherRefreshTime = DateTime.Now.AddHours(-2);
        _showEndTime = new TimeSpan(22, 15, 00);
        _weatherObservation = new();
    }

    public async Task<TimeSpan> UpdateWebsiteDisplayAsync()
    {
        const uint DELAY_DURATION = 10;
        FppStatusResponseDto currentStatus;
        const uint MIN_SECONDS_REMAINING = 5;

        try
        {
            currentStatus = await _fppHttpClient.GetFppdStatusAsync();

            if (currentStatus.Status_Name.ToUpper() == StatusName.Idle)
            {
                if (_previousStatus.Status_Name.ToUpper() == StatusName.Playing)
                {
                    EngineerDisplayRequestDto displayRequestDto = new();
                    await _engineerHttpClient.PostDisplayInfoAsync(displayRequestDto);
                }

                _previousStatus = currentStatus;
                return TimeSpan.FromSeconds(DELAY_DURATION);
            }
        }
        catch (Exception ex)
        {
            _logging.Error(ex, ex.Message);
            return TimeSpan.FromSeconds(DELAY_DURATION);
        }

        try
        {
            await GetWeatherObservationAsync();
        }
        catch (Exception ex)
        {
            _logging.Error(ex, ex.Message);
        }

        try
        {
            if (_previousStatus.Status_Name.ToUpper() == StatusName.Idle && currentStatus.Status_Name.ToUpper() == StatusName.Playing)
            {
                await _engineerHttpClient.DeleteAllSongsInQueueAsync();
            }

            await StopPlaylistAfterEndTimeAsync(currentStatus.Scheduler.CurrentPlaylist.Playlist);

            if (currentStatus.Current_Song == _previousStatus.Current_Song ||
                currentStatus.SecondsRemaining() > MIN_SECONDS_REMAINING)
            {
                _previousStatus = currentStatus;
                return TimeSpan.FromSeconds(DELAY_DURATION);
            }

            EngineerDisplayRequestDto engineerDisplayRequestDto = new();
            await GetAllCpuTemperaturesAsync(engineerDisplayRequestDto);
            engineerDisplayRequestDto.SetWindChill(_weatherObservation.Properties.WindChill.Value.ToDisplayTemperature());
            engineerDisplayRequestDto.SetNwsTempC(_weatherObservation.Properties.Temperature.Value.ToDisplayTemperature());
            await SetTitleAndArtistAsync(engineerDisplayRequestDto, currentStatus.Current_Song);
            _logging.Information(engineerDisplayRequestDto.ToString());

            await _engineerHttpClient.PostDisplayInfoAsync(engineerDisplayRequestDto);

            // todo - update other social media platforms

            if (currentStatus.Current_Song.ToUpper().Contains("PUBLIC SERVICE ANNOUNCEMENT") ||
                currentStatus.Current_Song.ToUpper().Contains("CODE "))
            {
                return TimeSpan.FromSeconds(currentStatus.SecondsRemaining() - 2);
            }

            EngineerResponseDto engineerResponseDto = await _engineerHttpClient.GetFirstUnplayedRequestAsync();
            if (engineerResponseDto.Message == string.Empty)
            {
                // todo - insert random song in playlist
                return TimeSpan.FromSeconds(DELAY_DURATION);
            }

            await InsertFppPlaylistAsync(engineerResponseDto.Message);

            _previousStatus = currentStatus;
        }
        catch (Exception ex)
        {
            _logging.Error(ex, ex.Message);
        }

        return TimeSpan.FromSeconds(DELAY_DURATION);
    }

    private async Task InsertFppPlaylistAsync(string sequenceFileName)
    {
        string fppResponse = await _fppHttpClient.GetInsertPlaylistAfterCurrent(sequenceFileName);
        if (fppResponse.ToUpper() != "PLAYLIST INSERTED")
        {
            throw new InvalidDataException($"Unexpected response from FPP. {fppResponse}");
        }
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

            string artist = string.IsNullOrWhiteSpace(metaResponse.Format.Tags.Artist) ?
                string.Empty : metaResponse.Format.Tags.Artist;
            displayDto.setArtist(artist);
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

    private async Task GetAllCpuTemperaturesAsync(EngineerDisplayRequestDto engineerDisplayRequestDto)
    {
        FppMultiSyncSystemsResponseDto fppMultiSyncSystemsDto = await _fppHttpClient.GetMultiSyncSystemsAsync();
        foreach (var system in fppMultiSyncSystemsDto.Systems)
        {
            var status = await _fppHttpClient.GetFppdStatusAsync(system.Address);

            float cpuTemperature = (float)status.Sensors
                .Where(s => s.Label.ToUpper().StartsWith("CPU"))
                .Select(s => s.Value)
                .Single();

            engineerDisplayRequestDto.AddCpuTemperature(cpuTemperature.ToDisplayTemperature());

            if (cpuTemperature >= _appSettings.FalconPlayer.MaxCpuTemperatureC)
            {
                _logging.Warning($"CPU temperature alert: {cpuTemperature.ToString()} for host {system.Hostname}");
            }
        }
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