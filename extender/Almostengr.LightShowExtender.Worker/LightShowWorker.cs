using Almostengr.LightShowExtender.DomainService.Website;
using Almostengr.LightShowExtender.DomainService.Website.Common;
using Almostengr.Common.NwsWeather;
using Almostengr.LightShowExtender.DomainService.Wled;
using Almostengr.LightShowExtender.DomainService.Common;
using Almostengr.LightShowExtender.DomainService.FalconPiPlayer;
using Almostengr.Common.Logging;
using Almostengr.Common.HomeAssistant;
using Microsoft.Extensions.Options;
using Almostengr.Common.HomeAssistant.Common;

namespace Almostengr.LightShowExtender.Worker;

internal sealed class LightShowWorker : BackgroundService
{
    private readonly ILoggingService<LightShowWorker> _loggingService;
    private readonly AppSettings _appSettings;
    private readonly IOptions<NwsOptions> _nwsOptions;
    private NwsLatestObservationResponse _weatherObservation;
    private uint _songsSincePsa;
    private DateTime _lastWeatherRefreshTime;
    private readonly IFppHttpClient _fppHttpClient;
    private readonly IWebsiteHttpClient _websiteHttpClient;
    private readonly IWledHttpClient _wledHttpClient;
    private readonly INwsHttpClient _nwsHttpClient;
    private FppStatusResponse _previousStatus;
    private readonly IHomeAssistantHttpClient _haHttpClient;

    public LightShowWorker(
        IFppHttpClient fppHttpClient,
        IWebsiteHttpClient websiteHttpClient,
        INwsHttpClient nwsHttpClient,
        IWledHttpClient wledHttpClient,
        IHomeAssistantHttpClient homeAssistantHttpClient,
        AppSettings appSettings,
        IOptions<NwsOptions> nwsOptions,
        ILoggingService<LightShowWorker> logging)
    {
        _appSettings = appSettings;
        _fppHttpClient = fppHttpClient;
        _websiteHttpClient = websiteHttpClient;
        _nwsHttpClient = nwsHttpClient;
        _wledHttpClient = wledHttpClient;
        _haHttpClient = homeAssistantHttpClient;
        _loggingService = logging;
        _nwsOptions = nwsOptions;
        _weatherObservation = new();
        _songsSincePsa = 0;
        _lastWeatherRefreshTime = DateTime.Now.AddHours(-2);
        _previousStatus = new();
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        TimeSpan delayTime;
        while (!cancellationToken.IsCancellationRequested)
        {
            delayTime = await MonitorAsync(cancellationToken);
            await Task.Delay(delayTime, cancellationToken);
        }
    }

    private int ConvertStringToInt(string value)
    {
        return Int32.Parse(value);
    }

    private async Task<TimeSpan> ServiceAsync(CancellationToken cancellationToken)
    {
        FppStatusResponse currentStatus;

        try
        {
            currentStatus = await GetStatusHandler.Handle(_fppHttpClient, cancellationToken);

            if (currentStatus.Current_Song == "" && _previousStatus.Current_Song == "")
            {
                return TimeSpan.FromSeconds(15);
            }
            else if (currentStatus.Current_Song == "" && _previousStatus.Current_Song != "")
            {
                await ShutdownShowAsync(currentStatus, cancellationToken);
            }
            else if (currentStatus.Current_Song != "" && _previousStatus.Current_Song == "")
            {
                await StartupShowAsync(currentStatus, cancellationToken);
            }

            TimeSpan timeDifference = DateTime.Now - _lastWeatherRefreshTime;
            if (timeDifference.Hours >= 1)
            {
                _weatherObservation = await GetLatestObservationHandler.Handle(_nwsHttpClient, _nwsOptions.Value.StationId, cancellationToken);
                _lastWeatherRefreshTime = DateTime.Now;
            }

            uint secondsRemaining = UInt32.Parse(currentStatus.Seconds_Remaining);
            const uint FETCH_TIME = 5;
            if (secondsRemaining >= FETCH_TIME)
            {
                _previousStatus = currentStatus;
                return TimeSpan.FromSeconds(secondsRemaining);
            }

            var nextSong = await GetNextSongInQueueHandler.Handle(_websiteHttpClient, cancellationToken);
            if (nextSong.Message == "")
            {
                return TimeSpan.FromSeconds(secondsRemaining);
            }


        }
        catch (Exception ex)
        {
            return TimeSpan.FromSeconds(5);
        }
    }

    private async Task<TimeSpan> MonitorAsync(CancellationToken cancellationToken)
    {
        FppStatusResponse currentStatus;

        try
        {
            currentStatus = await GetStatusHandler.Handle(_fppHttpClient, cancellationToken);

            int secondsRemaining = ConvertStringToInt(currentStatus.Seconds_Remaining);
            if (currentStatus.Current_Song == _previousStatus.Current_Song && secondsRemaining >= 3)
            {
                return TimeSpan.FromSeconds(secondsRemaining);
            }

            await StartupShowAsync(currentStatus, cancellationToken);
            await ShutdownShowAsync(currentStatus, cancellationToken);
        }
        catch (Exception ex)
        {
            _loggingService.Error(ex, ex.GetBaseException().ToString());
            return TimeSpan.FromSeconds(_appSettings.ExtenderDelay);
        }

        try
        {
            TimeSpan timeDifference = DateTime.Now - _lastWeatherRefreshTime;
            if (timeDifference.Hours >= 1)
            {
                _weatherObservation = await GetLatestObservationHandler.Handle(_nwsHttpClient, _nwsOptions.Value.StationId, cancellationToken);
                _lastWeatherRefreshTime = DateTime.Now;
            }
        }
        catch (Exception ex)
        {
            _loggingService.Error(ex, ex.GetBaseException().ToString());
        }

        try
        {
            await StopShowAfterEndTimeHandler.Handle(_fppHttpClient, currentStatus.Scheduler.CurrentPlaylist.Playlist, cancellationToken);

            FppMediaMetaResponse metaResponse = await GetCurrentSongMetaDataHandler.Handle(_fppHttpClient, currentStatus.Current_Song, cancellationToken);

            WebsiteDisplayInfoRequest displayRequest = await CreateDisplayRequestAsync(currentStatus, metaResponse, cancellationToken);
            await PostDisplayInfoHandler.Handle(_websiteHttpClient, displayRequest, cancellationToken);

            _songsSincePsa = currentStatus.Current_Song.Contains("PSA") ? 0 : _songsSincePsa;

            if (_songsSincePsa >= _appSettings.MaxSongsBetweenPsa)
            {
                await InsertPsaHandler.Handle(_fppHttpClient, cancellationToken);
                _songsSincePsa = 0;
                _previousSong = currentStatus.Current_Song;
                return TimeSpan.FromSeconds(_appSettings.ExtenderDelay);
            }

            var nextSongResponse = await GetNextSongInQueueHandler.Handle(_websiteHttpClient, cancellationToken);

            if (nextSongResponse.Message.IsNullOrWhiteSpace())
            {
                return TimeSpan.FromSeconds(_appSettings.ExtenderDelay);
            }

            await InsertPlaylistAfterCurrentHandler.Handle(_fppHttpClient, nextSongResponse.Message, cancellationToken);
            _songsSincePsa++;

            _previousSong = currentStatus.Current_Song;
        }
        catch (Exception ex)
        {
            _loggingService.Error(ex, ex.GetBaseException().ToString());
        }

        return TimeSpan.FromSeconds(_appSettings.ExtenderDelay);
    }

    private async Task<WebsiteDisplayInfoRequest> CreateDisplayRequestAsync(FppStatusResponse currentStatus, FppMediaMetaResponse metaResponse, CancellationToken cancellationToken)
    {
        string cpuTemperatures = await GetCpuTemperaturesHandler.Handle(_fppHttpClient, cancellationToken);
        string title = metaResponse.Format.Tags.Title.IsNullOrWhiteSpace() ? currentStatus.Current_Song.Replace(".mp3", string.Empty) : metaResponse.Format.Tags.Title;
        string weatherTemp = _weatherObservation.Properties.Temperature.Value.ToDisplayTemperature();
        string artist = metaResponse.Format.Tags.Artist ?? string.Empty;
        string windChill = _weatherObservation.Properties.WindChill.Value.ToDisplayTemperature();

        WebsiteDisplayInfoRequest displayRequest = new(title, weatherTemp, cpuTemperatures, artist, windChill);
        return displayRequest;
    }

    private async Task ShutdownShowAsync(FppStatusResponse currentStatus, CancellationToken cancellationToken)
    {
        if (_showOffline && currentStatus.Current_Song.Contains(_appSettings.ShutDownSequence))
        {
            var wledSystems = await GetMultiSyncSystemsHandler.Handle(_fppHttpClient, cancellationToken, "WLED");
            await TurnOffHandler.Handle(_wledHttpClient, wledSystems, cancellationToken);

            WebsiteDisplayInfoRequest displayRequest = new(string.Empty);
            await PostDisplayInfoHandler.Handle(_websiteHttpClient, displayRequest, cancellationToken);
            await TurnOnSwitchHandler.Handle(_haHttpClient, _appSettings.ExteriorLightEntity, cancellationToken);

            _previousSong = currentStatus.Current_Song;
            _showOffline = true;
        }
    }

    private async Task StartupShowAsync(FppStatusResponse currentStatus, CancellationToken cancellationToken)
    {
        if (_showOffline == true && currentStatus.Current_Song.Contains(_appSettings.StartupSequence))
        {
            var wledSystems = await GetMultiSyncSystemsHandler.Handle(_fppHttpClient, cancellationToken, "WLED");
            await TurnOnHandler.Handle(_wledHttpClient, wledSystems, cancellationToken);
            await DeleteSongsInQueueHandler.Handle(_websiteHttpClient, cancellationToken);
            await TurnOffSwitchHandler.Handle(_haHttpClient, _appSettings.ExteriorLightEntity, cancellationToken);

            _showOffline = false;
        }
    }
}
