
using Almostengr.Common.DomainServices;

namespace Almostengr.HpLightShow.WebApi.Features.StartSequence.DomainServices.Resources;

public sealed class SequenceSelectorResource : BaseResource
{
    public DateOnly CurrentDate { get; set; }
    public string Sequence { get; set; }
}