using Almostengr.LightShowExtender.DomainService.FalconPiPlayer;
using Almostengr.LightShowExtender.DomainService.Common;
using Almostengr.LightShowExtender.DomainService.TheAlmostEngineer;

namespace Almostengr.LightShowExtender.DomainService.Jukebox;

public sealed class JukeboxService : BaseService, IJukeboxService
{
    private readonly IFppHttpClient _fppHttpClient;
    private readonly IEngineerHttpClient _engineerHttpClient;
    private readonly ILoggingService<JukeboxService> _logger;
    private FppStatusResponseDto _previousStatus;
    private FppStatusResponseDto _currentStatus;

    public JukeboxService(IFppHttpClient fppHttpClient,
        IEngineerHttpClient engineerHttpClient,
        ILoggingService<JukeboxService> logger)
    {
        _fppHttpClient = fppHttpClient;
        _logger = logger;
        _engineerHttpClient = engineerHttpClient;
        _currentStatus = _previousStatus = new();
    }

    public async Task DelayBetweenRequestsAsync()
    {
        TimeSpan secondsRemaining = TimeSpan.FromSeconds(5);

        if (!string.IsNullOrWhiteSpace(_currentStatus.Seconds_Remaining))
        {
            secondsRemaining = TimeSpan.FromSeconds(Double.Parse(_currentStatus.Seconds_Remaining));
        }
        
        await Task.Delay(secondsRemaining);
    }

    public async Task UpdateJukeboxAsync()
    {
        try
        {
            _currentStatus = await _fppHttpClient.GetFppdStatusAsync();
            await ClearSongsInQueueAsync();
            _previousStatus = _currentStatus;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, ex.Message);
        }
    }

    private async Task ClearSongsInQueueAsync()
    {
        if (_previousStatus.Status_Name == StatusName.Idle &&
            _currentStatus.Status_Name == StatusName.Playing)
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

            string fppResponse = await _fppHttpClient.GetInsertPlaylistAfterCurrent(response.Message);
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
