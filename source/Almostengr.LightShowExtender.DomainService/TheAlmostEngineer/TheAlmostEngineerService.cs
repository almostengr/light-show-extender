using Almostengr.LightShowExtender.DomainService.Common.Constants;
using Almostengr.LightShowExtender.DomainService.FalconPiPlayer;
using Almostengr.LightShowExtender.DomainService.Common.Extensions;
using Almostengr.LightShowExtender.DomainService.Common;

namespace Almostengr.LightShowExtender.DomainService.TheAlmostEngineer;

public sealed class TheAlmostEngineerService : ITheAlmostEngineerService
{
    private readonly IFppHttpClient _fppHttpClient;
    private readonly ITaeHttpClient _taeHttpClient;
    private readonly ILoggingService<TheAlmostEngineerService> _logger;

    public TheAlmostEngineerService(IFppHttpClient fppHttpClient,
        ITaeHttpClient taeHttpClient,
        ILoggingService<TheAlmostEngineerService> logger)
    {
        _fppHttpClient = fppHttpClient;
        _logger = logger;
        _taeHttpClient = taeHttpClient;
    }

    public async Task<FppLatestStatusResult> UpdateCurrentSongAsync(FppLatestStatusResult fppLatestStatusResult)
    {
        try
        {
            var fppStatus = await _fppHttpClient.GetFppdStatusAsync(AppConstants.Localhost);

            TimeSpan secondsRemaining = TimeSpan.FromSeconds(Double.Parse(fppStatus.Seconds_Remaining));

            if (fppLatestStatusResult.LastPlaylist == "" && fppStatus.Current_PlayList.Playlist != "")
            {
                await _taeHttpClient.DeleteAllSongsInQueueAsync();
            }

            if (fppStatus.Current_Song == fppLatestStatusResult.LastSong ||
                fppStatus.Current_PlayList.Playlist.IsNullOrEmpty())
            {
                if (fppStatus.Status_Name == "idle")
                {
                    secondsRemaining = TimeSpan.FromSeconds(15);
                }

                return new FppLatestStatusResult(secondsRemaining, fppStatus.Current_Song, fppStatus.Current_PlayList.Playlist);
            }

            FppMediaMetaDto fppMediaMetaDto = await _fppHttpClient.GetCurrentSongMetaDataAsync(fppStatus.Current_Song);
            if (fppMediaMetaDto.IsNull())
            {
                return new FppLatestStatusResult(secondsRemaining, fppStatus.Current_Song, fppStatus.Current_PlayList.Playlist);
            }

            var settingDto = new TaeSettingDto(TaeSettingKey.CurrentSong.Value, fppMediaMetaDto.Format.Tags.Title);
            await _taeHttpClient.UpdateSettingAsync(settingDto);

            return new FppLatestStatusResult(secondsRemaining, fppStatus.Current_Song, fppStatus.Current_PlayList.Playlist);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, ex.Message);
            return new FppLatestStatusResult(
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

    public async Task<TimeSpan> UpdateCpuTemperatureAsync()
    {
        try
        {
            var status = await _fppHttpClient.GetFppdStatusAsync(AppConstants.Localhost);

            if (status.Current_Song.IsNullOrWhiteSpace())
            {
                return TimeSpan.FromMinutes(15);
            }

            var settingDto = new TaeSettingDto(
                TaeSettingKey.CpuTemperature.Value, status.Sensors[0].Value.ToString());

            await _taeHttpClient.UpdateSettingAsync(settingDto);

            return TimeSpan.FromMinutes(5);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, ex.Message);
            return TimeSpan.FromSeconds(AppConstants.ErrorDelaySeconds);
        }
    }
}