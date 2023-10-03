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
    const uint MIN_SECONDS_REMAINING = 5;

    public JukeboxService(IFppHttpClient fppHttpClient,
        IEngineerHttpClient engineerHttpClient,
        ILoggingService<JukeboxService> logger)
    {
        _fppHttpClient = fppHttpClient;
        _logger = logger;
        _engineerHttpClient = engineerHttpClient;
        _previousStatus = new();
    }

    public async Task<TimeSpan> ManageRequestsAsync()
    {
        try
        {
            var currentStatus = await _fppHttpClient.GetFppdStatusAsync();

            if (currentStatus.Current_Song == string.Empty)
            {
                return TimeSpan.FromSeconds(15);
            }

            await ClearJukeboxQueueWhenStartingAsync(currentStatus.Status_Name);

            _previousStatus = currentStatus;

            uint secondsRemaining = UInt32.Parse(currentStatus.Seconds_Remaining);
            if (secondsRemaining > MIN_SECONDS_REMAINING)
            {
                return TimeSpan.FromSeconds(secondsRemaining - MIN_SECONDS_REMAINING);
            }

            EngineerResponseDto response = await _engineerHttpClient.GetFirstUnplayedRequestAsync();
            if (response.Message == string.Empty)
            {
                return TimeSpan.FromSeconds(15);
                // var sequences = await _fppHttpClient.GetSequenceListAsync();
                // var filteredSequences = sequences.Where(s => !s.Contains("HPL")).ToList();

                // Random random = new();
                // response = new EngineerResponseDto
                // {
                //     Message = filteredSequences.ElementAt(random.Next(filteredSequences.Count()))
                // };
            }

            await InsertFppPlaylistAsync(response.Message);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, ex.Message);
        }

        return TimeSpan.FromSeconds(15);
    }

    private async Task InsertFppPlaylistAsync(string sequenceFileName)
    {
        string fppResponse = await _fppHttpClient.GetInsertPlaylistAfterCurrent(sequenceFileName);
        if (fppResponse.ToLower() != "playlist inserted")
        {
            throw new InvalidDataException($"Unexpected response from FPP. {fppResponse}");
        }
    }

    private async Task ClearJukeboxQueueWhenStartingAsync(string statusName)
    {
        if (statusName != string.Empty && _previousStatus.Current_Song == string.Empty)
        {
            await _engineerHttpClient.DeleteAllSongsInQueueAsync();
        }
    }
}
