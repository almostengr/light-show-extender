using Almostengr.Common.DomainServices;

namespace Almostengr.HpLightShow.Core.FalconPiPlayer.DomainServices;

public sealed class SequenceSelectorResource : BaseResource
{
    public SequenceSelectorResource(DateOnly currentDate)
    {
        CurrentDate = currentDate;
    }

    public DateOnly CurrentDate { get; init; }
}
