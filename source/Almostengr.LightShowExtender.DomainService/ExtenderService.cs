using Almostengr.Common.NwsWeather;
using Almostengr.LightShowExtender.DomainService.Wled;
using Almostengr.LightShowExtender.DomainService.Common;
using Almostengr.LightShowExtender.DomainService.FalconPiPlayer;
using Almostengr.Common.TheAlmostEngineer;
using Almostengr.Common.Logging;
using Almostengr.Common.HomeAssistant;
using Microsoft.Extensions.Options;

namespace Almostengr.LightShowExtender.DomainService;

public sealed class ExtenderService : IExtenderService
{
    private readonly ILoggingService<ExtenderService> _loggingService;
    private readonly IFppService _fppService;
    private readonly AppSettings _appSettings;
    private NwsLatestObservationResponse _weatherObservation;
    private readonly INwsService _nwsService;
    private readonly ILightShowService _engineerService;
    private readonly IHomeAssistantService _homeAssistantService;
    private readonly IOptions<NwsOptions> _nwsOptions;
    private readonly IWledService _wledService;
    private string _previousSong;
    private uint _songsSincePsa;
    private bool _showOffline;

    public ExtenderService(
        IFppService fppService,
        INwsService nwsService,
        ILightShowService engineerService,
        IHomeAssistantService homeAssistantService,
        IWledService wledService,
        AppSettings appSettings,
        IOptions<NwsOptions> nwsOptions,
        ILoggingService<ExtenderService> logging)
    {
        _appSettings = appSettings;
        _loggingService = logging;
        _fppService = fppService;
        _nwsService = nwsService;
        _wledService = wledService;
        _engineerService = engineerService;
        _homeAssistantService = homeAssistantService;
        _nwsOptions = nwsOptions;
        _weatherObservation = new();
        _previousSong = "START";
        _songsSincePsa = 0;
        _showOffline = true;
    }

    public async Task<TimeSpan> MonitorAsync(CancellationToken cancellationToken)
    {
        FppStatusResponse currentStatus;

        try
        {
            currentStatus = await _fppService.GetFppdStatusAsync(cancellationToken);

            string currentSequence = currentStatus.Current_Sequence.ToUpper();
            if (currentSequence.Contains(_appSettings.StartupSequence.ToUpper()))
            {
                await ShowStartupAsync(currentStatus, cancellationToken);
            }
            else if (currentSequence.Contains(_appSettings.ShutDownSequence.ToUpper()))
            {
                await ShowShutdownAsync(currentStatus, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _loggingService.Error(ex, ex.GetBaseException().ToString());
            return TimeSpan.FromSeconds(_appSettings.ExtenderDelay);
        }

        if (_showOffline)
        {
            return TimeSpan.FromSeconds(_appSettings.ExtenderDelay);
        }

        try
        {
            _weatherObservation = await _nwsService.GetLatestObservationAsync(_nwsOptions.Value.StationId, cancellationToken);
        }
        catch (Exception ex)
        {
            _loggingService.Error(ex, ex.GetBaseException().ToString());
        }

        try
        {
            await _fppService.StopPlaylistAfterEndTimeAsync(currentStatus.Scheduler.CurrentPlaylist.Playlist, cancellationToken);

            if (currentStatus.Current_Song == _previousSong)
            {
                return TimeSpan.FromSeconds(_appSettings.ExtenderDelay);
            }

            FppMediaMetaResponse metaResponse = await _fppService.GetCurrentSongMetaDataAsync(currentStatus.Current_Song, cancellationToken);

            string cpuTemperatures = await _fppService.GetCpuTemperaturesAsync(cancellationToken);
            string title = metaResponse.Format.Tags.Title.IsNullOrWhiteSpace() ? currentStatus.Current_Song.Replace(".mp3", string.Empty) : metaResponse.Format.Tags.Title;
            string weatherTemp = _weatherObservation.Properties.Temperature.Value.ToDisplayTemperature();
            string artist = metaResponse.Format.Tags.Artist ?? string.Empty;
            string windChill = _weatherObservation.Properties.WindChill.Value.ToDisplayTemperature();

            LightShowDisplayRequest displayRequest = new(title, weatherTemp, cpuTemperatures, artist, windChill);
            await _engineerService.PostDisplayInfoAsync(displayRequest, cancellationToken);

            _songsSincePsa = currentStatus.Current_Song.Contains("PSA") ? 0 : _songsSincePsa;

            if (_songsSincePsa >= _appSettings.MaxSongsBetweenPsa)
            {
                await _fppService.InsertPsaAsync(cancellationToken);
                _songsSincePsa = 0;
                _previousSong = currentStatus.Current_Song;
                return TimeSpan.FromSeconds(_appSettings.ExtenderDelay);
            }

            var nextSongResponse = await _engineerService.GetNextSongInQueueAsync(cancellationToken);

            if (nextSongResponse.Message.IsNullOrWhiteSpace())
            {
                return TimeSpan.FromSeconds(_appSettings.ExtenderDelay);
            }

            await _fppService.InsertPlaylistAfterCurrentAsync(nextSongResponse.Message, cancellationToken);
            _songsSincePsa++;

            _previousSong = currentStatus.Current_Song;
        }
        catch (Exception ex)
        {
            _loggingService.Error(ex, ex.GetBaseException().ToString());
        }

        return TimeSpan.FromSeconds(_appSettings.ExtenderDelay);
    }

    private async Task ShowShutdownAsync(FppStatusResponse currentStatus, CancellationToken cancellationToken)
    {
        List<string> wledSystems = await _fppService.GetWledSystemsFromMultiSyncSystemsAsync(cancellationToken);
        await _wledService.TurnOffAsync(wledSystems, cancellationToken);

        // await _homeAssistantService.TurnOnSwitchAsync(_appSettings.ExteriorLightEntity, cancellationToken);

        LightShowDisplayRequest displayRequest = new(string.Empty);
        await _engineerService.PostDisplayInfoAsync(displayRequest, cancellationToken);

        _previousSong = currentStatus.Current_Song;

        _showOffline = true;
    }

    private async Task ShowStartupAsync(FppStatusResponse currentStatus, CancellationToken cancellationToken)
    {
        List<string> wledSystems = await _fppService.GetWledSystemsFromMultiSyncSystemsAsync(cancellationToken, true);
        await _wledService.TurnOnAsync(wledSystems, cancellationToken);

        // await _homeAssistantService.TurnOffSwitchAsync(_appSettings.ExteriorLightEntity, cancellationToken);

        await _engineerService.DeleteQueueWhenPlaylistStartsAsync(_previousSong, currentStatus.Current_Song, cancellationToken);

        _showOffline = false;
    }
}


public interface IExtenderService
{
    Task<TimeSpan> MonitorAsync(CancellationToken cancellationToken);
}