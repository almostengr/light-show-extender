using Almostengr.LightShowExtender.DomainService.Common.Constants;
using Almostengr.LightShowExtender.DomainService.FalconPiPlayer;
using Almostengr.LightShowExtender.DomainService.Common.Extensions;
using Almostengr.LightShowExtender.DomainService.Common;

namespace Almostengr.LightShowExtender.DomainService.TheAlmostEngineer;

public sealed class TheAlmostEngineerService : ITheAlmostEngineerService
{
    private readonly IFppHttpClient _fppHttpClient;
    private readonly ILoggingService<TheAlmostEngineerService> _logger;

    public TheAlmostEngineerService(IFppHttpClient fppHttpClient,
        ILoggingService<TheAlmostEngineerService> logger)
    {
        _fppHttpClient = fppHttpClient;
        _logger = logger;
    }

    public async Task<(string lastSong, TimeSpan delay)> UpdateCurrentSongAsync(string lastSong)
    {
        try
        {
            var fppStatus = await _fppHttpClient.GetFppdStatusAsync(AppConstants.Localhost);

            TimeSpan secondsRemaining = TimeSpan.FromSeconds(Double.Parse(fppStatus.Seconds_Remaining));

            if (fppStatus.Current_Song == lastSong ||
                fppStatus.Current_PlayList.Playlist.IsNullOrEmpty())
            {
                if (fppStatus.Status_Name == "idle")
                {
                    secondsRemaining = TimeSpan.FromSeconds(15);
                }

                return new(fppStatus.Current_Song, secondsRemaining);
            }

            FppMediaMetaDto fppMediaMetaDto = await _fppHttpClient.GetCurrentSongMetaDataAsync(fppStatus.Current_Song);
            if (fppMediaMetaDto.IsNull())
            {
                return new(fppStatus.Current_Song, secondsRemaining);
            }

            // post call to website

            return new(string.Empty, secondsRemaining);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, ex.Message);
            return new(string.Empty, TimeSpan.FromSeconds(AppConstants.ErrorDelaySeconds));
        }
    }

    public async Task GetLatestJukeboxRequest()
    {
        try
        {
            // get latest request from website

            string songName = "";
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

            var settingDto = new TaeSettingDto("cputemperature", status.Sensors[0].Value.ToString());

            // make call to website

            return TimeSpan.FromMinutes(5);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, ex.Message);
            return TimeSpan.FromSeconds(AppConstants.ErrorDelaySeconds);
        }
    }
}