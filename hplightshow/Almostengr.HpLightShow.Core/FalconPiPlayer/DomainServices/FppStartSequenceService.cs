using Almostengr.Common.DomainServices.Results;
using Almostengr.FalconPiPlayerClient.Domain;
using Almostengr.FalconPiPlayerClient.DomainServices.Interfaces;
using Almostengr.HpLightShow.Core.FalconPiPlayer.Domain;
using Almostengr.HpLightShow.Core.FalconPiPlayer.DomainServices.Interfaces;
using Almostengr.HpLightShow.Core.FalconPiPlayer.Resources.DomainServices;
using Almostengr.HpLightShow.Core.FalconPiPlayer.Shared;

namespace Almostengr.HpLightShow.Core.FalconPiPlayer.DomainServices;

public sealed class FppStartSequenceService : IFppStartSequenceService
{
    private readonly FppAppSettings _appSettings;
    private readonly IFppSequenceRepository _repository;
    private readonly IFppdHttpClient _fppClient;

    public FppStartSequenceService(
        FppAppSettings appSettings,
        IFppSequenceRepository repository,
        IFppdHttpClient fppClient
        )
    {
        _appSettings = appSettings;
        _repository = repository;
        _fppClient = fppClient;
    }

    public async Task<Result<SequenceSelectorResource>> ExecuteAsync(SequenceSelectorResource resource, bool commitTransaction = true)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(resource, nameof(resource));

            var status = await _fppClient.GetStatusAsync();
            if (status.Status != FppStatusOption.Idle)
            {
                return Result<SequenceSelectorResource>.Success(resource);
            }

            if (!string.IsNullOrWhiteSpace(_appSettings.SequenceOverride))
            {
                await _fppClient.StartPlaylistAsync(_appSettings.SequenceOverride);
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
            return Result<SequenceSelectorResource>.Failure(ex);
        }
    }
}
