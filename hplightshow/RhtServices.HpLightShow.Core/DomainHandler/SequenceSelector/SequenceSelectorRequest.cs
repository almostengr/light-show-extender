using RhtServices.Common.Utilities.DomainService;

namespace RhtServices.HpLightShow.Core.DomainHandler.SequenceSelector;

public sealed class SequenceSelectorRequest : IHandlerRequest
{
    public SequenceSelectorRequest(DateOnly currentDate)
    {
        CurrentDate = currentDate;
    }

    public DateOnly CurrentDate { get; init; }
}