using Almostengr.Common.Infrastructure;
using Almostengr.Common.OperationResult;
using Almostengr.HpLightShow.Core.FalconPiPlayer.Common;
using Almostengr.HpLightShow.Core.FalconPiPlayer.DataTransferObjects;
using Almostengr.HpLightShow.Core.FalconPiPlayer.Enums;

namespace Almostengr.HpLightShow.Core.FalconPiPlayer.Services;

public sealed class FppdService : BaseService, IFppdService
{
    public FppdService(HttpClient httpClient) : base(httpClient)
    {
    }

    private async Task<FppStatusDto> GetFppdStatusAsync()
    {
        string route = "api/fppd/status";
        return await _httpClient.GetAsync<FppStatusDto>(route);
    }

    public async Task<ServiceResult<string>> MonitorAsync()
    {
        FppStatusDto fppStatus = await GetFppdStatusAsync() ?? throw new InvalidOperationException("Error when retrieving status from FPP.");
        if (fppStatus.Status == (int)FppStatusType.Idle)
        {
            return ServiceResult<string>.Success(string.Empty);
        }

        var result = ServiceResult<string>.Create();

        if (fppStatus.Warnings.Count > 0)
        {
            result.AddError("Warnings were found.");
        }

        return result;
    }
}
