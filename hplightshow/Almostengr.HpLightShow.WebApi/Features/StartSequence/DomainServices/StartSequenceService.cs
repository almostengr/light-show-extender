using Almostengr.Common.DomainServices.Results;
using Almostengr.HpLightShow.WebApi.Features.Monitoring.Domain;
using Almostengr.HpLightShow.WebApi.Features.Monitoring.DomainServices.Interfaces;
using Almostengr.HpLightShow.WebApi.Features.StartSequence.DomainServices.Interfaces;
using Almostengr.HpLightShow.WebApi.Features.StartSequence.DomainServices.Resources;

namespace Almostengr.HpLightShow.WebApi.Features.StartSequence.DomainServices;

public sealed class StartSequenceService : IStartSequenceService
{
    private readonly IFppSequenceRepository _repository;
    private readonly ILogger<StartSequenceService> _logger;
    private readonly IFppdHttpClient _fppClient;

    public StartSequenceService(
        ILogger<StartSequenceService> logger,
        IFppSequenceRepository repository,
        IFppdHttpClient fppClient
    )
    {
        _fppClient = fppClient;
        _logger = logger;
        _repository = repository;
    }

    public async Task<Result<SequenceSelectorResource>> ExecuteAsync(SequenceSelectorResource resource, bool commitTransaction = true)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(resource, nameof(resource));

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
