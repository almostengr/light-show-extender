using Almostengr.Common.OperationResult;
using Almostengr.HpLightShow.Core.Common;
using Almostengr.HpLightShow.Core.Common.Common;
using Almostengr.HpLightShow.Core.FalconPiPlayer.Enums;
using Almostengr.HpLightShow.Core.FalconPiPlayer.Infrastructure;
using Almostengr.HpLightShow.Core.FalconPiPlayer.Resources;
using Almostengr.HpLightShow.Core.Wled.Infrastructure;
using Almostengr.HpLightShow.Core.Wled.Resources;

namespace Almostengr.HpLightShow.Core.FalconPiPlayer.Services;

public sealed class FppMonitorService : IFppMonitorService
{
    private readonly AppSettings _appSettings;
    private readonly ISocialMediaPoster _socialMediaPoster;
    private readonly IWledClient _wledClient;
    private readonly IFppClient _fppClient;

    public FppMonitorService(
        AppSettings appSettings,
        ISocialMediaPoster socialMediaPoster,
        IWledClient wledClient,
        IFppClient fppClient
    )
    {
        _appSettings = appSettings;
        _socialMediaPoster = socialMediaPoster;
        _wledClient = wledClient;
        _fppClient = fppClient;
    }

    public async Task<Result<FppMonitorResource>> ExecuteAsync(FppMonitorResource resource)
    {
        try
        {
            Result<FppMonitorResource> result = Result<FppMonitorResource>.Create();

            FppStatusResource fppStatus = await _fppClient.GetFppdStatusAsync() ?? throw new InvalidOperationException("Error when retrieving status from FPP.");
            if (fppStatus.Status == (int)FppStatusType.Idle)
            {
                MultiSyncSystemsResource mulitSyncStatus = await _fppClient.MultiSyncSystemsResource();
                if (mulitSyncStatus == null)
                {
                    result.AddError("Unable to get mulitsync status from FPP.");
                    return result;
                }

                await CheckWledInstancesAsync(result, mulitSyncStatus);
            }
            else
            {
                CheckWarnings(result, fppStatus);
                CheckCpuTemperature(result, fppStatus);
            }

            if (result.Failed)
            {
                await _socialMediaPoster.PostAsync($"Check system. {result.Errors.Count()} error(s) reported.");
            }

            return result;
        }
        catch (Exception ex)
        {
            return Result<FppMonitorResource>.Failure(ex);
        }
    }

    private async Task CheckWledInstancesAsync(Result<FppMonitorResource> result, MultiSyncSystemsResource mulitSyncStatus)
    {
        foreach (var status in mulitSyncStatus.Systems)
        {
            WledStatusResource wledStatus = await _wledClient.GetStatusAsync(status.Address);
            if (wledStatus == null)
            {
                result.AddError($"Unable to reach WLED instance. {status.Hostname}");
                continue;
            }

            if (wledStatus.State.On == true)
            {
                WledStatusResource updateResource = new();
                WledStatusResource updateResult = await _wledClient.UpdateStatusAsync(updateResource, status.Address);

                if (updateResult.State.On == false)
                {
                    result.AddError($"Unable to turn off WLED instance {status.Hostname}");
                }
            }
        }
    }

    private void CheckWarnings(Result<FppMonitorResource> result, FppStatusResource fppStatus)
    {
        _ = fppStatus ?? throw new ArgumentNullException(nameof(fppStatus));

        if (fppStatus.Warnings.Count > 0)
        {
            result.AddError("Warnings were found.");
        }
    }

    private void CheckCpuTemperature(Result<FppMonitorResource> result, FppStatusResource fppStatus)
    {
        _ = fppStatus ?? throw new ArgumentNullException(nameof(fppStatus));

        foreach (FppStatusResource.Sensor sensor in fppStatus.Sensors)
        {
            if (sensor.Label.Contains("CPU", StringComparison.OrdinalIgnoreCase) &&
                sensor.Value > _appSettings.MaxCpuTemperatureC)
            {
                result.AddError($"High CPU temperature {sensor.Value}.");
            }
        }
    }
}
