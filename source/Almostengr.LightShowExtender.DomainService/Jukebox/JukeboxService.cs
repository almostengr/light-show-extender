using Almostengr.LightShowExtender.DomainService.Common.Constants;
using Almostengr.LightShowExtender.DomainService.FalconPiPlayer;
using Almostengr.LightShowExtender.DomainService.Common.Extensions;
using Almostengr.LightShowExtender.DomainService.Common;
using Almostengr.LightShowExtender.DomainService.TheAlmostEngineer;

namespace Almostengr.LightShowExtender.DomainService.Jukebox;

public sealed class JukeboxService : IJukeboxService
{
    private readonly IFppHttpClient _fppHttpClient;
    private readonly IEngineerHttpClient _engineerHttpClient;
    private readonly ILoggingService<JukeboxService> _logger;

    public JukeboxService(IFppHttpClient fppHttpClient,
        IEngineerHttpClient engineerHttpClient,
        ILoggingService<JukeboxService> logger)
    {
        _fppHttpClient = fppHttpClient;
        _logger = logger;
        _engineerHttpClient = engineerHttpClient;
    }

    public async Task<LatestJukeboxStateDto> UpdateCurrentSongAsync(LatestJukeboxStateDto latestJukeboxStateDto)
    {
        try
        {
            var fppStatus = await _fppHttpClient.GetFppdStatusAsync();

            TimeSpan secondsRemaining = TimeSpan.FromSeconds(Double.Parse(fppStatus.Seconds_Remaining));

            if (latestJukeboxStateDto.LastPlaylist == "" && fppStatus.Current_Playlist.Playlist != "")
            {
                await _engineerHttpClient.DeleteAllSongsInQueueAsync();
            }

            if (fppStatus.Current_Song == latestJukeboxStateDto.LastSong ||
                fppStatus.Current_Playlist.Playlist.IsNullOrEmpty())
            {
                if (fppStatus.Status_Name == "idle")
                {
                    secondsRemaining = TimeSpan.FromSeconds(15);
                }

                return new LatestJukeboxStateDto(secondsRemaining, fppStatus.Current_Song, fppStatus.Current_Playlist.Playlist);
            }

            FppMediaMetaDto fppMediaMetaDto = await _fppHttpClient.GetCurrentSongMetaDataAsync(fppStatus.Current_Song);
            if (fppMediaMetaDto.IsNull())
            {
                return new LatestJukeboxStateDto(secondsRemaining, fppStatus.Current_Song, fppStatus.Current_Playlist.Playlist);
            }

            string requestValue = $"{fppMediaMetaDto.Format.Tags.Title}|{fppMediaMetaDto.Format.Tags.Artist}";
            var settingDto = new EngineerSettingRequestDto(EngineerSettingKey.CurrentSong.Value, requestValue);
            await _engineerHttpClient.UpdateSettingAsync(settingDto);

            return new LatestJukeboxStateDto(secondsRemaining, fppStatus.Current_Song, fppStatus.Current_Playlist.Playlist);
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
            EngineerResponseDto response = await _engineerHttpClient.GetFirstUnplayedRequestAsync();
            string songName = "";  // todo update below when response data is finalized
            await _fppHttpClient.GetInsertPlaylistAfterCurrent(songName);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, ex.Message);
        }
    }
}
