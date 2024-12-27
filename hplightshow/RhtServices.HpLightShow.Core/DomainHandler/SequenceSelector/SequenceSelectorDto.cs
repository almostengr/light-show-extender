using RhtServices.Common.Utilities.DomainService;

namespace RhtServices.HpLightShow.Core.DomainHandler.SequenceSelector;

public sealed class SequenceSelectorDto : IHandlerDto
{
    public SequenceSelectorDto(DateOnly currentDate)
    {
        CurrentDate = currentDate;
    }

    public DateOnly CurrentDate { get; init; }
}