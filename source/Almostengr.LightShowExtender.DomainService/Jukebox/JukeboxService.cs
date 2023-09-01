using Almostengr.LightShowExtender.DomainService.Common.Constants;
using Almostengr.LightShowExtender.DomainService.FalconPiPlayer;
using Almostengr.LightShowExtender.DomainService.Common.Extensions;
using Almostengr.LightShowExtender.DomainService.Common;
using Almostengr.LightShowExtender.DomainService.TheAlmostEngineer;

namespace Almostengr.LightShowExtender.DomainService.Jukebox;

public sealed class JukeboxService : IJukeboxService
{
    private readonly IFppHttpClient _fppHttpClient;
    private readonly ITaeHttpClient _taeHttpClient;
    private readonly ILoggingService<JukeboxService> _logger;

    public JukeboxService(IFppHttpClient fppHttpClient,
        ITaeHttpClient taeHttpClient,
        ILoggingService<JukeboxService> logger)
    {
        _fppHttpClient = fppHttpClient;
        _logger = logger;
        _taeHttpClient = taeHttpClient;
    }

    public async Task<LatestJukeboxStateDto> UpdateCurrentSongAsync(LatestJukeboxStateDto latestJukeboxStateDto)
    {
        try
        {
            var fppStatus = await _fppHttpClient.GetFppdStatusAsync(AppConstants.Localhost);

            TimeSpan secondsRemaining = TimeSpan.FromSeconds(Double.Parse(fppStatus.Seconds_Remaining));

            if (latestJukeboxStateDto.LastPlaylist == "" && fppStatus.Current_PlayList.Playlist != "")
            {
                await _taeHttpClient.DeleteAllSongsInQueueAsync();
            }

            if (fppStatus.Current_Song == latestJukeboxStateDto.LastSong ||
                fppStatus.Current_PlayList.Playlist.IsNullOrEmpty())
            {
                if (fppStatus.Status_Name == "idle")
                {
                    secondsRemaining = TimeSpan.FromSeconds(15);
                }

                return new LatestJukeboxStateDto(secondsRemaining, fppStatus.Current_Song, fppStatus.Current_PlayList.Playlist);
            }

            FppMediaMetaDto fppMediaMetaDto = await _fppHttpClient.GetCurrentSongMetaDataAsync(fppStatus.Current_Song);
            if (fppMediaMetaDto.IsNull())
            {
                return new LatestJukeboxStateDto(secondsRemaining, fppStatus.Current_Song, fppStatus.Current_PlayList.Playlist);
            }

            var settingDto = new TaeSettingDto(TaeSettingKey.CurrentSong.Value, fppMediaMetaDto.Format.Tags.Title);
            await _taeHttpClient.UpdateSettingAsync(settingDto);

            return new LatestJukeboxStateDto(secondsRemaining, fppStatus.Current_Song, fppStatus.Current_PlayList.Playlist);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, ex.Message);
            return new LatestJukeboxStateDto(
                TimeSpan.FromSeconds(AppConstants.ErrorDelaySeconds),
                string.Empty,
                string.Empty);
        }
    }

    public async Task GetLatestJukeboxRequest()
    {
        try
        {
            TaeResponseDto response = await _taeHttpClient.GetFirstUnplayedRequestAsync();
            string songName = "";  // todo update below when response data is finalized
            await _fppHttpClient.GetInsertPlaylistAfterCurrent(songName);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, ex.Message);
        }
    }


}