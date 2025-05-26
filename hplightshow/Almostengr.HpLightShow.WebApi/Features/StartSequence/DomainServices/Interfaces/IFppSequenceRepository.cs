using Almostengr.HpLightShow.WebApi.Features.Monitoring.Domain;

namespace Almostengr.HpLightShow.WebApi.Features.StartSequence.DomainServices.Interfaces;

public interface IFppSequenceRepository
{
    IList<SequenceRule> GetSequenceRules(int currentYear);
}