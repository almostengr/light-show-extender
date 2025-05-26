using Almostengr.Common.DomainServices.Results;
using Almostengr.HpLightShow.WebApi.Features.Monitoring.DomainServices.Interfaces;
using Almostengr.HpLightShow.WebApi.Features.Monitoring.DomainServices.Resources;
using Almostengr.HpLightShow.WebApi.Models;

namespace Almostengr.HpLightShow.WebApi.Workers;

internal sealed class MonitorWorker : BackgroundService
{
    private readonly AppSettings _appSettings;
    private readonly IMonitorService _monitorService;
    private readonly ILogger<MonitorWorker> _logger;

    public MonitorWorker(
        AppSettings appSettings,
        ILogger<MonitorWorker> logger,
        IMonitorService monitorService
        )
    {
        _appSettings = appSettings;
        _monitorService = monitorService;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            MonitorResource resource = new();
            resource.MaxCpuTemperatureC = _appSettings.MaxCpuTemperatureC;

            Result<MonitorResource> fppResult = await _monitorService.ExecuteAsync(resource);
            if (fppResult.Failed)
            {
                fppResult.Errors.ToList().ForEach(e => _logger.LogError(e));
            }

            await Task.Delay(TimeSpan.FromMinutes(_appSettings.MonitorDelay), stoppingToken);
        }
    }
}
