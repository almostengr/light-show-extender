using Almostengr.Common.OperationResult;

namespace Almostengr.HpLightShow.Core.FalconPiPlayer.Services;

public interface IFppdService
{
    Task<Result<int>> MonitorAsync();
}