using Almostengr.Common.DomainServices;

namespace Almostengr.HpLightShow.Core.FalconPiPlayer.Resources.DomainServices;

public sealed class SequenceSelectorResource : BaseDomainResource
{
    public SequenceSelectorResource(DateOnly currentDate)
    {
        CurrentDate = currentDate;
    }

    public DateOnly CurrentDate { get; set; }
    public string? Sequence { get; init; } = null;
}
