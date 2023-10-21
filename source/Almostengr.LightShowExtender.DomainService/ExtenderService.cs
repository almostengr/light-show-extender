using Almostengr.Common.NwsWeather;
using Almostengr.LightShowExtender.DomainService.Common;
using Almostengr.LightShowExtender.DomainService.FalconPiPlayer;
using Almostengr.Common.TheAlmostEngineer;
using Almostengr.Common.Logging;
using Almostengr.Common.HomeAssistant;

namespace Almostengr.LightShowExtender.DomainService;

public sealed class ExtenderService : IExtenderService
{
    private readonly ILoggingService<ExtenderService> _logging;
    private readonly IFppService _fppService;
    private readonly AppSettings _appSettings;
    private NwsLatestObservationResponse _weatherObservation;
    private readonly INwsService _nwsService;
    private readonly ILightShowService _engineerService;
    private readonly IHomeAssistantService _homeAssistantService;
    private string _previousSong;
    private uint _songsSincePsa;

    public ExtenderService(
        IFppService fppService,
        INwsService nwsService,
        ILightShowService engineerService,
        IHomeAssistantService homeAssistantService,
        AppSettings appSettings,
        ILoggingService<ExtenderService> logging)
    {
        _appSettings = appSettings;
        _logging = logging;
        _fppService = fppService;
        _nwsService = nwsService;
        _engineerService = engineerService;
        _homeAssistantService = homeAssistantService;
        _weatherObservation = new();
        _previousSong = "START";
        _songsSincePsa = 0;
    }

    public async Task<TimeSpan> MonitorAsync(CancellationToken cancellationToken)
    {
        const string DRIVEWAY_SWITCH = "switch.driveway";
        FppStatusResponse currentStatus;

        try
        {
            currentStatus = await _fppService.GetFppdStatusAsync(cancellationToken);

            if (currentStatus.Current_Song.IsNullOrWhiteSpace())
            {
                if (_previousSong.IsNotNullOrWhiteSpace())
                {
                    LightShowDisplayRequest displayRequest = new(string.Empty);
                    await _engineerService.PostDisplayInfoAsync(displayRequest, cancellationToken);

                    // todo - turn off lights ran by WLED

                    await _homeAssistantService.TurnOnSwitchAsync(DRIVEWAY_SWITCH, cancellationToken);
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
            _weatherObservation = await _nwsService.GetLatestObservationAsync("KMGM", cancellationToken);
        }
        catch (Exception ex)
        {
            _logging.Error(ex, ex.GetBaseException().ToString());
        }

        try
        {
            await _engineerService.DeleteQueueWhenPlaylistStartsAsync(_previousSong, currentStatus.Current_Song, cancellationToken);
            await _fppService.StopPlaylistAfterEndTimeAsync(currentStatus.Scheduler.CurrentPlaylist.Playlist, cancellationToken);

            if (currentStatus.Current_Song == _previousSong)
            {
                _previousSong = currentStatus.Current_Song;
                return TimeSpan.FromSeconds(_appSettings.ExtenderDelay);
            }

            string songTItle = string.Empty;
            string songArtist = string.Empty;
            FppMediaMetaResponse metaResponse = await _fppService.GetCurrentSongMetaDataAsync(currentStatus.Current_Song, cancellationToken);
            if (metaResponse != null)
            {

                songTItle = string.IsNullOrWhiteSpace(metaResponse.Format.Tags.Title) ?
                    _fppService.GetSongNameFromFileName(currentStatus.Current_Song) : metaResponse.Format.Tags.Title;

                songArtist = string.IsNullOrWhiteSpace(metaResponse.Format.Tags.Artist) ?
                   string.Empty : metaResponse.Format.Tags.Artist;
            }

            LightShowDisplayRequest displayRequest =
                new(currentStatus.Current_Song,
                    _weatherObservation.Properties.Temperature.Value.ToString() ?? string.Empty,
                    "",
                    songArtist,
                    _weatherObservation.Properties.WindChill.Value.ToString() ?? string.Empty);
            await _engineerService.PostDisplayInfoAsync(displayRequest, cancellationToken);

            if (_songsSincePsa >= _appSettings.MaxSongsBetweenPsa)
            {
                List<string> allSequences = await _fppService.GetSequenceListAsync(cancellationToken);
                string psaSequence = allSequences.Where(s => s.ToUpper().Contains("PSA")).First();
                psaSequence += ".fseq";
                LightShowDisplayResponse psaRequest = new LightShowDisplayResponse(psaSequence);
                await _fppService.InsertPlaylistAfterCurrentAsync(psaRequest.Message, cancellationToken);
                _songsSincePsa = 0;
                _previousSong = currentStatus.Current_Song;
                return TimeSpan.FromSeconds(_appSettings.ExtenderDelay);
            }

            var unplayedRequest = await _engineerService.GetNextSongInQueueAsync(cancellationToken);

            if (unplayedRequest.Message.IsNullOrWhiteSpace())
            {
                return TimeSpan.FromSeconds(_appSettings.ExtenderDelay);
            }

            await _fppService.InsertPlaylistAfterCurrentAsync(unplayedRequest.Message, cancellationToken);
            _songsSincePsa++;

            _previousSong = currentStatus.Current_Song;
        }
        catch (Exception ex)
        {
            _logging.Error(ex, ex.GetBaseException().ToString());
        }

        return TimeSpan.FromSeconds(_appSettings.ExtenderDelay);
    }
}


public interface IExtenderService
{
    Task<TimeSpan> MonitorAsync(CancellationToken cancellationToken);
}