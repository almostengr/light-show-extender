using Almostengr.LightShowExtender.DomainService.Common.Constants;
using Almostengr.LightShowExtender.DomainService.FalconPiPlayer;
using Almostengr.LightShowExtender.DomainService.Common;
using Almostengr.LightShowExtender.DomainService.TheAlmostEngineer;

namespace Almostengr.LightShowExtender.DomainService.Jukebox;

public sealed class JukeboxService : BaseService, IJukeboxService
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

    public async Task<PreviousJukeboxStateDto> UpdateJukeboxAsync(PreviousJukeboxStateDto previousJukeboxStateDto)
    {
        try
        {
            var fppStatus = await _fppHttpClient.GetFppdStatusAsync();
            await ClearSongsInQueueAsync(previousJukeboxStateDto, fppStatus);
            await UpdateCurrentSongDisplayAsync(previousJukeboxStateDto, fppStatus);

            TimeSpan secondsRemaining = TimeSpan.FromSeconds(Double.Parse(fppStatus.Seconds_Remaining));
            return new PreviousJukeboxStateDto(secondsRemaining, fppStatus.Current_Song, fppStatus.Status_Name);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, ex.Message);
            return new PreviousJukeboxStateDto(
                TimeSpan.FromSeconds(AppConstants.ErrorDelaySeconds),
                previousJukeboxStateDto.LastSong,
                previousJukeboxStateDto.StatusName);
        }
    }

    private string GetSongNameFromFileName(string value)
    {
        value = Path.GetFileNameWithoutExtension(value)
            .Replace("_", " ").Replace("-", " ");
        return value;
    }

    private async Task UpdateCurrentSongDisplayAsync(PreviousJukeboxStateDto previousJukeboxStateDto, FppStatusDto fppStatus)
    {
        if (fppStatus.Current_Song == previousJukeboxStateDto.LastSong)
        {
            return;
        }

        string requestValue = string.Empty;

        if (fppStatus.Current_Song != string.Empty)
        {
            const int TESTING_VOLUME_THRESHOLD = 50;
            if (fppStatus.Volume > TESTING_VOLUME_THRESHOLD)
            {
                FppMediaMetaDto fppMediaMetaDto = await _fppHttpClient.GetCurrentSongMetaDataAsync(fppStatus.Current_Song);

                requestValue = fppMediaMetaDto == null ?
                    GetSongNameFromFileName(fppStatus.Current_Song) :
                    $"{fppMediaMetaDto.Format.Tags.Title}|{fppMediaMetaDto.Format.Tags.Artist}";
            }
        }

        var settingDto = new EngineerSettingRequestDto(EngineerSettingKey.CurrentSong.Value, requestValue);
        await _engineerHttpClient.UpdateSettingAsync(settingDto);
    }

    private async Task ClearSongsInQueueAsync(PreviousJukeboxStateDto previousJukeboxStateDto, FppStatusDto fppStatus)
    {
        if (previousJukeboxStateDto.StatusName == StatusName.Idle &&
            fppStatus.Status_Name == StatusName.Playing)
        {
            await _engineerHttpClient.DeleteAllSongsInQueueAsync();
        }
    }

    public async Task GetLatestJukeboxRequest()
    {
        try
        {
            EngineerResponseDto response = await _engineerHttpClient.GetFirstUnplayedRequestAsync();
            if (response.Message == string.Empty)
            {
                return;
            }

            var fppResponse = await _fppHttpClient.GetInsertPlaylistAfterCurrent(response.Message);
            if (fppResponse.ToLower() != "playlist inserted")
            {
                throw new InvalidDataException($"Unexpected response from FPP. {fppResponse}");
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, ex.Message);
        }
    }

}


