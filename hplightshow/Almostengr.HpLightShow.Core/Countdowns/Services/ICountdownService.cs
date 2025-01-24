using Almostengr.Common.OperationResult;

namespace Almostengr.HpLightShow.Core.Countdowns;

public interface ICountdownService
{
    Task<Result<int>> PostCountdownAsync(HolidayCountdownDto countdownDto);
}