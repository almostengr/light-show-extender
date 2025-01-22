using Almostengr.HpLightShow.Core.SequenceSelector.DataTransferObjects;

namespace Almostengr.HpLightShow.Core.SequenceSelector.Services;

public interface ISequenceSelectorService
{
    Task ExecuteAsync(SequenceSelectorDto selectorDto);
}