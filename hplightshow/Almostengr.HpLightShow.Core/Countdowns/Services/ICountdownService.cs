using Almostengr.Common.OperationResult;

namespace Almostengr.HpLightShow.Core.Countdowns;

public interface ICountdownService
{
    Task<ServiceResult<int>> PostCountdownAsync(HolidayCountdownDto countdownDto);
}