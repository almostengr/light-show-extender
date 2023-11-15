using Almostengr.LightShowExtender.DomainService.Website;
using Almostengr.Common.NwsWeather;
using Almostengr.LightShowExtender.DomainService.Wled;
using Almostengr.LightShowExtender.DomainService.Common;
using Almostengr.LightShowExtender.DomainService.FalconPiPlayer;
using Almostengr.Extensions.Logging;
using Almostengr.Common.HomeAssistant;
using Microsoft.Extensions.Options;

namespace Almostengr.LightShowExtender.Worker;

internal sealed class ExtenderWorker : BackgroundService
{
    private readonly ILoggingService<ExtenderWorker> _loggingService;
    private readonly AppSettings _appSettings;
    private readonly IOptions<NwsOptions> _nwsOptions;
    private NwsLatestObservationResponse _weatherObservation;
    private uint _songsSincePsa;
    private DateTime _lastWeatherRefreshTime;
    private FppStatusResponse _previousStatus;

    private readonly DeleteSongsInQueueHandler _deleteSongsInQueueHandler;
    private readonly GetCpuTemperaturesHandler _getCpuTemperaturesHandler;
    private readonly GetCurrentSongMetaDataHandler _getCurrentSongMetaDataHandler;
    private readonly GetLatestObservationHandler _getLatestObservationHandler;
    private readonly GetMultiSyncSystemsHandler _getMultiSyncSystemsHandler;
    private readonly GetNextSongInQueueHandler _getNextSongInQueueHandler;
    private readonly GetStatusHandler _getStatusHandler;
    private readonly InsertPlaylistAfterCurrentHandler _insertPlaylistAfterCurrentHandler;
    private readonly InsertPsaHandler _insertPsaHandler;
    private readonly PostDisplayInfoHandler _postDisplayInfoHandler;
    private readonly StopShowAfterEndTimeHandler _stopShowAfterEndTimeHandler;
    private readonly TurnOffWledHandler _turnOffWledHandler;
    private readonly TurnOffSwitchHandler _turnOffSwitchHandler;
    private readonly TurnOnWledHandler _turnOnWledHandler;
    private readonly TurnOnSwitchHandler _turnOnSwitchHandler;

    public ExtenderWorker(
        AppSettings appSettings,
        IOptions<NwsOptions> nwsOptions,
        ILoggingService<ExtenderWorker> logging,
        DeleteSongsInQueueHandler deleteSongsInQueueHandler,
        GetCpuTemperaturesHandler getCpuTemperaturesHandler,
        GetCurrentSongMetaDataHandler getCurrentSongMetaDataHandler,
        GetLatestObservationHandler getLatestObservationHandler,
        GetMultiSyncSystemsHandler getMultiSyncSystemsHandler,
        GetNextSongInQueueHandler getNextSongInQueueHandler,
        GetStatusHandler getStatusHandler,
        InsertPlaylistAfterCurrentHandler insertPlaylistAfterCurrentHandler,
        InsertPsaHandler insertPsaHandler,
        PostDisplayInfoHandler postDisplayInfoHandler,
        StopShowAfterEndTimeHandler stopShowAfterEndTimeHandler,
        TurnOffWledHandler turnOffHandler,
        TurnOffSwitchHandler turnOffSwitchHandler,
        TurnOnWledHandler turnOnHandler,
        TurnOnSwitchHandler turnOnSwitchHandler
        )
    {
        _appSettings = appSettings;
        _nwsOptions = nwsOptions;
        _loggingService = logging;
        _weatherObservation = new();
        _songsSincePsa = 0;
        _lastWeatherRefreshTime = DateTime.Now.AddHours(-2);
        _previousStatus = new();

        _deleteSongsInQueueHandler = deleteSongsInQueueHandler;
        _getCpuTemperaturesHandler = getCpuTemperaturesHandler;
        _getCurrentSongMetaDataHandler = getCurrentSongMetaDataHandler;
        _getLatestObservationHandler = getLatestObservationHandler;
        _getMultiSyncSystemsHandler = getMultiSyncSystemsHandler;
        _getNextSongInQueueHandler = getNextSongInQueueHandler;
        _getStatusHandler = getStatusHandler;
        _insertPlaylistAfterCurrentHandler = insertPlaylistAfterCurrentHandler;
        _insertPsaHandler = insertPsaHandler;
        _postDisplayInfoHandler = postDisplayInfoHandler;
        _stopShowAfterEndTimeHandler = stopShowAfterEndTimeHandler;
        _turnOffWledHandler = turnOffHandler;
        _turnOffSwitchHandler = turnOffSwitchHandler;
        _turnOnWledHandler = turnOnHandler;
        _turnOnSwitchHandler = turnOnSwitchHandler;
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

    private async Task<TimeSpan> MonitorAsync(CancellationToken cancellationToken)
    {
        FppStatusResponse currentStatus = await _getStatusHandler.Handle(cancellationToken);

        if (currentStatus == null)
        {
            _loggingService.Error("Unable to get FPP status");
            return TimeSpan.FromSeconds(15);
        }

        await ShutdownShowBasedOnConditionsAsync(currentStatus, cancellationToken);
        await StartupShowBasedOnConditionsAsync(currentStatus, cancellationToken);

        if (currentStatus.Current_Song == "")
        {
            _previousStatus = currentStatus;
            return TimeSpan.FromSeconds(15);
        }

        await GetLatestWeatherAsync(cancellationToken);

        FppMediaMetaResponse metaResponse = await _getCurrentSongMetaDataHandler.Handle(currentStatus.Current_Song, cancellationToken);

        WebsiteDisplayInfoRequest displayRequest = await CreateDisplayRequestAsync(currentStatus, metaResponse, cancellationToken);
        await _postDisplayInfoHandler.Handle(displayRequest, cancellationToken);

        uint secondsRemaining = ConvertStringToUint(currentStatus.Seconds_Remaining);
        const uint FETCH_TIME = 5;
        if (secondsRemaining > FETCH_TIME)
        {
            _previousStatus = currentStatus;
            return TimeSpan.FromSeconds(secondsRemaining - FETCH_TIME);
        }

        _songsSincePsa = currentStatus.Current_Song.ToUpper().Contains("PSA") ? 0 : _songsSincePsa;
        if (_songsSincePsa >= _appSettings.MaxSongsBetweenPsa)
        {
            await _insertPsaHandler.Handle(cancellationToken);
            _songsSincePsa = 0;
            _previousStatus = currentStatus;
            return TimeSpan.FromSeconds(_appSettings.ExtenderDelay);
        }

        var nextSong = await _getNextSongInQueueHandler.Handle(cancellationToken);
        if (string.IsNullOrWhiteSpace(nextSong.Message))
        {
            _previousStatus = currentStatus;
            return TimeSpan.FromSeconds(FETCH_TIME);
        }

        await _insertPlaylistAfterCurrentHandler.Handle(nextSong.Message, cancellationToken);
        _songsSincePsa++;
        _previousStatus = currentStatus;

        return TimeSpan.FromSeconds(FETCH_TIME);
    }

    private uint ConvertStringToUint(string value)
    {
        return UInt32.Parse(value);
    }
    
    private async Task GetLatestWeatherAsync(CancellationToken cancellationToken)
    {
        TimeSpan timeDifference = DateTime.Now - _lastWeatherRefreshTime;
        if (timeDifference.Hours >= 1)
        {
            _weatherObservation = await _getLatestObservationHandler.Handle(_nwsOptions.Value.StationId, cancellationToken);
            _lastWeatherRefreshTime = DateTime.Now;
        }
    }

    private async Task<WebsiteDisplayInfoRequest> CreateDisplayRequestAsync(FppStatusResponse currentStatus, FppMediaMetaResponse metaResponse, CancellationToken cancellationToken)
    {
        string cpuTemperatures = await _getCpuTemperaturesHandler.Handle(cancellationToken);
        string title = string.IsNullOrWhiteSpace(metaResponse.Format.Tags.Title) ? currentStatus.Current_Song.Replace(".mp3", string.Empty) : metaResponse.Format.Tags.Title;
        string weatherTemp = _weatherObservation.Properties.Temperature.Value.ToDisplayTemperature();
        string artist = metaResponse.Format.Tags.Artist ?? string.Empty;
        string windChill = _weatherObservation.Properties.WindChill.Value.ToDisplayTemperature();

        WebsiteDisplayInfoRequest displayRequest = new(title, true, weatherTemp, cpuTemperatures, artist, windChill);
        return displayRequest;
    }

    private async Task ShutdownShowBasedOnConditionsAsync(FppStatusResponse currentStatus, CancellationToken cancellationToken)
    {
        if (currentStatus.Current_Song == "" && _previousStatus.Current_Song != "")
        {
            var wledSystems = await _getMultiSyncSystemsHandler.Handle(cancellationToken, "WLED");
            await _turnOffWledHandler.Handle(wledSystems, cancellationToken);

            WebsiteDisplayInfoRequest displayRequest = new(string.Empty, false);
            await _postDisplayInfoHandler.Handle(displayRequest, cancellationToken);

            TurnOnSwitchRequest switchRequest = new(_appSettings.ExteriorLightEntity);
            await _turnOnSwitchHandler.Handle(switchRequest, cancellationToken);

            _previousStatus = currentStatus;
        }
    }

    private async Task StartupShowBasedOnConditionsAsync(FppStatusResponse currentStatus, CancellationToken cancellationToken)
    {
        if (currentStatus.Current_Song != "" && _previousStatus.Current_Song == "")
        {
            var wledSystems = await _getMultiSyncSystemsHandler.Handle(cancellationToken, "WLED");
            await _turnOnWledHandler.Handle(wledSystems, cancellationToken);

            TurnOffSwitchRequest switchRequest = new(_appSettings.ExteriorLightEntity);
            await _turnOffSwitchHandler.HandleAsync(switchRequest, cancellationToken);

            await _deleteSongsInQueueHandler.Handle(cancellationToken);
        }
    }
}
