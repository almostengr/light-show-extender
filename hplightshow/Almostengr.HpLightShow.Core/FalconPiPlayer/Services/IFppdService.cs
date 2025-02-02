using Almostengr.Common.OperationResult;
using Almostengr.HpLightShow.Core.re;

namespace Almostengr.HpLightShow.Core.FalconPiPlayer.Services;

public interface IFppdService
{
    Task<Result<int>> MonitorAsync();
    Task<Result<int>> StartSequenceAsync(SequenceSelectorResource selectorDto);
}