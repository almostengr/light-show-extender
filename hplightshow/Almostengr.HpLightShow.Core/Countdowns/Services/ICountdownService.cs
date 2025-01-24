using Almostengr.Common.OperationResult;
using Almostengr.HpLightShow.Core.Countdowns.DataTransferObjects;

namespace Almostengr.HpLightShow.Core.Countdowns.Service;

public interface ICountdownService
{
    Task<Result<int>> PostCountdownAsync(HolidayCountdownDto countdownDto);
}