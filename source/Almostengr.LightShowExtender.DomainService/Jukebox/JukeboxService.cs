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

            if (latestJukeboxStateDto.StatusName == StatusName.Idle &&
                fppStatus.Status_Name == StatusName.Playing)
            {
                await _engineerHttpClient.DeleteAllSongsInQueueAsync();
            }

            TimeSpan secondsRemaining = TimeSpan.FromSeconds(Double.Parse(fppStatus.Seconds_Remaining));

            if (fppStatus.Current_Song != latestJukeboxStateDto.LastSong)
            {
                FppMediaMetaDto fppMediaMetaDto = await _fppHttpClient.GetCurrentSongMetaDataAsync(fppStatus.Current_Song);

                string requestValue = fppMediaMetaDto == null ?
                    fppStatus.Current_Song.Replace(".mp3", "") :
                    $"{fppMediaMetaDto.Format.Tags.Title}|{fppMediaMetaDto.Format.Tags.Artist}";

                var settingDto = new EngineerSettingRequestDto(EngineerSettingKey.CurrentSong.Value, requestValue);
                await _engineerHttpClient.UpdateSettingAsync(settingDto);
            }

            return new LatestJukeboxStateDto(secondsRemaining, fppStatus.Current_Song, fppStatus.Status_Name);
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
            string songName = response.Message;
            await _fppHttpClient.GetInsertPlaylistAfterCurrent(songName);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, ex.Message);
        }
    }

    internal static class StatusName
    {
        public static string Idle = "Idle";
        public static string Playing = "Playing";
    }
}


