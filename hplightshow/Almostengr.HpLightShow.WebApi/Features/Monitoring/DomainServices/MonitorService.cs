using Almostengr.Common.DomainServices.Interfaces;
using Almostengr.Common.DomainServices.Results;
using Almostengr.HpLightShow.WebApi.Features.Monitoring.Domain;
using Almostengr.HpLightShow.WebApi.Features.Monitoring.DomainServices.Interfaces;
using Almostengr.HpLightShow.WebApi.Features.SocialMediaPosts.DomainServices.Resources;

namespace Almostengr.HpLightShow.WebApi.Features.Monitoring.DomainServices.Resources;

internal sealed class MonitorService : IMonitorService
{
    private readonly IFppdHttpClient _fppClient;
    private readonly ILogger<MonitorService> _logger;
    private readonly ICommandService<SocialMediaResource> _socialMediaService;
    private readonly IWledClient _wledClient;

    public MonitorService(
        ILogger<MonitorService> logger,
        IFppdHttpClient fppClient,
        ICommandService<SocialMediaResource> socialMediaService,
        IWledClient wledClient
    )
    {
        _fppClient = fppClient;
        _logger = logger;
        _socialMediaService = socialMediaService;
        _wledClient = wledClient;
    }

    public async Task<Result<MonitorResource>> ExecuteAsync(MonitorResource resource, bool commitTransaction = true)
    {
        try
        {
            FppdStatusResource fppStatus = await _fppClient.GetStatusAsync();
            if (fppStatus == null)
            {
                return Result<MonitorResource>.Failure("Error when retrieving status from FPP.");
            }

            Result<MonitorResource> result = Result<MonitorResource>.Create();
            if (fppStatus.Status == (int)FppStatusOption.Idle)
            {
                FppMultiSyncSystemsResource mulitSyncStatus = await _fppClient.GetMultiSyncSystemsAsync();
                if (mulitSyncStatus == null)
                {
                    result.AddError("Unable to get mulitsync status from FPP.");
                    return result;
                }

                await CheckWledInstancesAsync(result, mulitSyncStatus);
            }

            CheckWarnings(result, fppStatus);
            CheckCpuTemperature(result, fppStatus, resource.MaxCpuTemperatureC);

            if (result.Failed)
            {
                SocialMediaResource socialMediaResource = new();
                socialMediaResource.Text = $"Check system. {result.Errors.Count()} error(s) reported.";
                await _socialMediaService.ExecuteAsync(socialMediaResource);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Result<MonitorResource>.Failure(ex);
        }
    }

    private async Task CheckWledInstancesAsync(Result<MonitorResource> result, FppMultiSyncSystemsResource mulitSyncStatus)
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

    private void CheckWarnings(Result<MonitorResource> result, FppdStatusResource fppStatus)
    {
        ArgumentNullException.ThrowIfNull(fppStatus, nameof(fppStatus));

        if (fppStatus.Warnings.Count > 0)
        {
            result.AddError("Warnings were found.");
        }
    }

    private void CheckCpuTemperature(Result<MonitorResource> result, FppdStatusResource fppStatus, double maxCpuTemperature)
    {
        ArgumentNullException.ThrowIfNull(fppStatus, nameof(fppStatus));

        foreach (FppdStatusResource.Sensor sensor in fppStatus.Sensors)
        {
            if (sensor.Label.Contains("CPU", StringComparison.OrdinalIgnoreCase) &&
                sensor.Value > maxCpuTemperature)
            {
                result.AddError($"High CPU temperature {sensor.Value}.");
            }
        }
    }
}
