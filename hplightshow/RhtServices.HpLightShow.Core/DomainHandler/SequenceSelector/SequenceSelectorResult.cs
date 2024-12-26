using RhtServices.Common.Utilities.DomainService;

namespace RhtServices.HpLightShow.Core.DomainHandler.SequenceSelector;

public sealed class SequenceSelectorResult : HandlerResult, IHandlerResult
{
    public SequenceSelectorResult(bool succeeded) : base(succeeded)
    {
    }
}