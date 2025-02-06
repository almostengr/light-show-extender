using Almostengr.Common.Resources;

namespace Almostengr.HpLightShow.Core.Resources;

public sealed class SequenceSelectorResource : BaseResource
{
    public SequenceSelectorResource(DateOnly currentDate)
    {
        CurrentDate = currentDate;
    }

    public DateOnly CurrentDate { get; init; }
}