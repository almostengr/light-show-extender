using Almostengr.HpLightShow.Core.FalconPiPlayer.Domain;

namespace Almostengr.HpLightShow.Core.FalconPiPlayer.DomainServices.Interfaces;

public interface IFppSequenceRepository
{
    IList<SequenceRule> GetSequenceRules(int currentYear);
}
