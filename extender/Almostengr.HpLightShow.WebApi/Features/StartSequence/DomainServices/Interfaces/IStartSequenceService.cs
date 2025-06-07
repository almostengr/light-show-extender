using Almostengr.Common.DomainServices.Interfaces;
using Almostengr.HpLightShow.WebApi.Features.StartSequence.DomainServices.Resources;

namespace Almostengr.HpLightShow.WebApi.Features.StartSequence.DomainServices;

public interface IStartSequenceService : ICommandService<SequenceSelectorResource>;
