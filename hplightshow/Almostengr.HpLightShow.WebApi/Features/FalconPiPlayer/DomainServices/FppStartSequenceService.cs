using Almostengr.Common.DomainServices.Results;
using Almostengr.FalconPiPlayerClient.Domain;
using Almostengr.FalconPiPlayerClient.DomainServices.Interfaces;
using Almostengr.FalconPiPlayerClient.DomainServices.Resources;
using Almostengr.HpLightShow.Core.FalconPiPlayer.Domain;
using Almostengr.HpLightShow.Core.FalconPiPlayer.DomainServices.Interfaces;
using Almostengr.HpLightShow.Core.FalconPiPlayer.Resources.DomainServices;
using Microsoft.Extensions.Logging;

namespace Almostengr.HpLightShow.Core.FalconPiPlayer.DomainServices;

public sealed class FppStartSequenceService : IFppStartSequenceService
{
    private readonly IFppSequenceRepository _repository;
    private readonly ILogger<FppStartSequenceService> _logger;
    private readonly IFppdHttpClient _fppClient;

    public FppStartSequenceService(
        ILogger<FppStartSequenceService> logger,
        IFppSequenceRepository repository,
        IFppdHttpClient fppClient
        )
    {
        _repository = repository;
        _logger = logger;
        _fppClient = fppClient;
    }

    public async Task<Result<SequenceSelectorResource>> ExecuteAsync(SequenceSelectorResource resource, bool commitTransaction = true)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(resource, nameof(resource));

            FppdStatusResource status = await _fppClient.GetStatusAsync();
            if (status.Status != FppStatusOption.Idle)
            {
                // return Result<SequenceSelectorResource>.Failure("Sequence is already playing.");
            }

            if (!string.IsNullOrWhiteSpace(resource.Sequence))
            {
                await _fppClient.StartPlaylistAsync(resource.Sequence);
                return Result<SequenceSelectorResource>.Success(resource);
            }

            IList<SequenceRule> rules = _repository.GetSequenceRules(resource.CurrentDate.Year);
            string selectedSequence = LightingSequence.Blue.Value;
            foreach (var rule in rules)
            {
                if (resource.CurrentDate >= rule.StartDate &&
                    resource.CurrentDate <= rule.EndDate &&
                    (rule.DaysOfWeek == null || rule.DaysOfWeek.Contains(resource.CurrentDate.DayOfWeek)))
                {
                    selectedSequence = rule.Sequence.Value;
                    break;
                }
            }

            await _fppClient.StartPlaylistAsync(selectedSequence);
            return Result<SequenceSelectorResource>.Success(resource);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Result<SequenceSelectorResource>.Failure(ex);
        }
    }
}
