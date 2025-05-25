using Almostengr.Common.DomainServices.Interfaces;
using Almostengr.HpLightShow.Core.FalconPiPlayer.Resources.DomainServices;

namespace Almostengr.HpLightShow.Core.FalconPiPlayer.DomainServices;

public interface IFppStartSequenceService : ICommandService<SequenceSelectorResource>;
