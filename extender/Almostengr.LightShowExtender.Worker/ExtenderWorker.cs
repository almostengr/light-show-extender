using Almostengr.LightShowExtender.DomainService.TweetInvi;
using Almostengr.NationalWeatherService;
using Almostengr.Wled.DomainService
using Almostengr.LightShowExtender.DomainService.Common;
using Almostengr.LightShowExtender.DomainService.FalconPiPlayer;
using Almostengr.Extensions.Logging;

namespace Almostengr.LightShowExtender.Worker;

internal sealed class ExtenderWorker : BackgroundService
{
    private readonly ILoggingService<ExtenderWorker> _loggingService;
    private readonly AppSettings _appSettings;
    private NwsLatestObservationResponse _weatherObservation;
    private uint _songsSincePsa;
    private uint _songsSinceLastTweet;
    private DateTime _lastWeatherRefreshTime;
    private string _previousSong;

    private readonly DeleteSongsInQueueHandler _deleteSongsInQueueHandler;
    private readonly GetCpuTemperaturesHandler _getCpuTemperaturesHandler;
    private readonly GetCurrentSongMetaDataHandler _getCurrentSongMetaDataHandler;
    private readonly GetLatestObservationQueryHandler _getLatestObservationHandler;
    private readonly GetMultiSyncSystemsHandler _getMultiSyncSystemsHandler;
    private readonly GetNextSongInQueueHandler _getNextSongInQueueHandler;
    private readonly GetStatusHandler _getStatusHandler;
    private readonly InsertPlaylistAfterCurrentHandler _insertPlaylistAfterCurrentHandler;
    private readonly InsertPsaHandler _insertPsaHandler;
    private readonly PostDisplayInfoHandler _postDisplayInfoHandler;
    private readonly PostTweetHandler _postTweetHandler;
    private readonly TurnOffWledHandler _turnOffWledHandler;
    private readonly TurnOnWledHandler _turnOnWledHandler;

    public ExtenderWorker(
        AppSettings appSettings,
        ILoggingService<ExtenderWorker> logging,
        DeleteSongsInQueueHandler deleteSongsInQueueHandler,
        GetCpuTemperaturesHandler getCpuTemperaturesHandler,
        GetCurrentSongMetaDataHandler getCurrentSongMetaDataHandler,
        GetLatestObservationQueryHandler getLatestObservationHandler,
        GetMultiSyncSystemsHandler getMultiSyncSystemsHandler,
        GetNextSongInQueueHandler getNextSongInQueueHandler,
        GetStatusHandler getStatusHandler,
        InsertPlaylistAfterCurrentHandler insertPlaylistAfterCurrentHandler,
        InsertPsaHandler insertPsaHandler,
        PostDisplayInfoHandler postDisplayInfoHandler,
        PostTweetHandler postTweetHandler,
        StopShowAfterEndTimeHandler stopShowAfterEndTimeHandler,
        TurnOffWledHandler turnOffHandler,
        TurnOnWledHandler turnOnHandler
        )
    {
        _appSettings = appSettings;
        _loggingService = logging;
        _weatherObservation = new();
        _songsSincePsa = 0;
        _songsSinceLastTweet = _appSettings.MaxSongsBetweenPsa;
        _lastWeatherRefreshTime = DateTime.Now.AddHours(-2);

        _postTweetHandler = postTweetHandler;
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
        _turnOffWledHandler = turnOffHandler;
        _turnOnWledHandler = turnOnHandler;
        _previousSong = null!;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        TimeSpan delayTime;
        while (!cancellationToken.IsCancellationRequested)
        {
            // delayTime = await MonitorAsync(cancellationToken);

            

            await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
        }
    }

    private async Task<TimeSpan> MonitorAsync(CancellationToken cancellationToken)
    {
        FppStatusRequest currentStatusRequest = new(string.Empty);
        FppStatusResponse currentStatus = await _getStatusHandler.ExecuteAsync(currentStatusRequest, cancellationToken);

        if (currentStatus == null)
        {
            _loggingService.Error("Unable to get FPP status");
            return TimeSpan.FromSeconds(15);
        }

        await ShutdownShowBasedOnConditionsAsync(currentStatus.Current_Song, cancellationToken);
        await StartupShowBasedOnConditionsAsync(currentStatus.Current_Song, cancellationToken);

        if (currentStatus.Current_Song == string.Empty)
        {
            UpdatePreviousSong(currentStatus.Current_Song);
            return TimeSpan.FromSeconds(15);
        }

        await GetLatestWeatherAsync(cancellationToken);

        FppMediaMetaRequest metaRequest = new(currentStatus.Current_Song);
        FppMediaMetaResponse metaResponse = await _getCurrentSongMetaDataHandler.ExecuteAsync(metaRequest, cancellationToken);

        WebsiteDisplayInfoRequest displayRequest = await CreateDisplayRequestAsync(currentStatus, metaResponse, cancellationToken);
        await _postDisplayInfoHandler.ExecuteAsync(displayRequest, cancellationToken);

        _songsSinceLastTweet++;
        if (_songsSinceLastTweet >= _appSettings.SongsBetweenTweets)
        {
            PostTweetCommand tweetCommand = new(displayRequest.Title, displayRequest.Artist);
            await _postTweetHandler.ExecuteAsync(tweetCommand, cancellationToken);
            _songsSinceLastTweet = 0;
        }

        uint secondsRemaining = ConvertStringToUint(currentStatus.Seconds_Remaining);
        if (secondsRemaining >= _appSettings.ExtenderDelay)
        {
            UpdatePreviousSong(currentStatus.Current_Song);
            return TimeSpan.FromSeconds(secondsRemaining - _appSettings.ExtenderDelay);
        }

        _songsSincePsa = currentStatus.Current_Song.ToUpper().Contains("PSA") ? 0 : _songsSincePsa;
        if (_songsSincePsa >= _appSettings.MaxSongsBetweenPsa)
        {
            await _insertPsaHandler.ExecuteAsync(cancellationToken);
            _songsSincePsa = 0;
            UpdatePreviousSong(currentStatus.Current_Song);
            return TimeSpan.FromSeconds(_appSettings.ExtenderDelay);
        }

        var nextSong = await _getNextSongInQueueHandler.ExecuteAsync(cancellationToken);
        if (string.IsNullOrWhiteSpace(nextSong.Message))
        {
            UpdatePreviousSong(currentStatus.Current_Song);
            return TimeSpan.FromSeconds(_appSettings.ExtenderDelay);
        }

        InsertPlaylistAfterCurrentRequest nextSongRequest = new(nextSong.Message);
        await _insertPlaylistAfterCurrentHandler.ExecuteAsync(nextSongRequest, cancellationToken);
        _songsSincePsa++;
        UpdatePreviousSong(currentStatus.Current_Song);

        return TimeSpan.FromSeconds(_appSettings.ExtenderDelay);
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
            var latestResponse = await _getLatestObservationHandler.ExecuteAsync(cancellationToken);
            if (latestResponse != null)
            {
                _weatherObservation = latestResponse;
                _lastWeatherRefreshTime = DateTime.Now;
            }
        }
    }

    private async Task<WebsiteDisplayInfoRequest> CreateDisplayRequestAsync(FppStatusResponse currentStatus, FppMediaMetaResponse metaResponse, CancellationToken cancellationToken)
    {
        string cpuTemperatures = await _getCpuTemperaturesHandler.ExecuteAsync(cancellationToken);
        string title = string.IsNullOrWhiteSpace(metaResponse.Format.Tags.Title) ? currentStatus.Current_Song.Replace(".mp3", string.Empty) : metaResponse.Format.Tags.Title;
        string weatherTemp = _weatherObservation.Properties.Temperature.Value.ToDisplayTemperature();
        string artist = metaResponse.Format.Tags.Artist ?? string.Empty;
        string windChill = _weatherObservation.Properties.WindChill.Value.ToDisplayTemperature();

        WebsiteDisplayInfoRequest displayRequest = new(title, true, weatherTemp, cpuTemperatures, artist, windChill);
        return displayRequest;
    }

    private async Task ShutdownShowBasedOnConditionsAsync(string currentSong, CancellationToken cancellationToken)
    {
        if (currentSong == "" && _previousSong != "")
        {
            var wledSystems = await _getMultiSyncSystemsHandler.ExecuteAsync("WLED", cancellationToken);
            await _turnOffWledHandler.ExecuteAsync(wledSystems, cancellationToken);

            WebsiteDisplayInfoRequest displayRequest = new(string.Empty, false);
            await _postDisplayInfoHandler.ExecuteAsync(displayRequest, cancellationToken);

            // TurnOnSwitchRequest switchRequest = new(_appSettings.ExteriorLightEntity);
            // await _turnOnSwitchHandler.ExecuteAsync(switchRequest, cancellationToken);  // todo when HA configured

            UpdatePreviousSong(currentSong);

            PostTweetCommand tweetCommand = new($"Light show is now offline.");
            await _postTweetHandler.ExecuteAsync(tweetCommand, cancellationToken);
        }
    }

    private void UpdatePreviousSong(string currentSong)
    {
        _previousSong = currentSong;
    }

    private async Task StartupShowBasedOnConditionsAsync(string currentSong, CancellationToken cancellationToken)
    {
        if (currentSong != "" && _previousSong == "")
        {
            var wledSystems = await _getMultiSyncSystemsHandler.ExecuteAsync("WLED", cancellationToken);
            await _turnOnWledHandler.ExecuteAsync(wledSystems, cancellationToken);

            // TurnOffSwitchRequest switchRequest = new(_appSettings.ExteriorLightEntity);
            // await _turnOffSwitchHandler.ExecuteAsync(switchRequest, cancellationToken);  // todo when HA configured

            await _deleteSongsInQueueHandler.ExecuteAsync(cancellationToken);
        }
    }
}
