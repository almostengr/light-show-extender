using Almostengr.Common.DomainServices.Results;
using Almostengr.HpLightShow.Core.FalconPiPlayer.DomainServices;
using Almostengr.HpLightShow.Core.FalconPiPlayer.DomainServices.Interfaces;
using Almostengr.HpLightShow.Core.FalconPiPlayer.Resources.DomainServices;

namespace Almostengr.HpLightShow.Worker;

internal sealed class FppMonitorWorker : BackgroundService
{
    private readonly IFppMonitorService _fppdService;
    private readonly IFppStartSequenceService _startSequenceService;
    private readonly ILogger<FppMonitorWorker> _logger;

    public FppMonitorWorker(
        IFppMonitorService fppdService,
        IFppStartSequenceService startSequenceService,
        ILogger<FppMonitorWorker> logger
        )
    {
        _fppdService = fppdService;
        _startSequenceService = startSequenceService;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            FppMonitorResource resource = new();
            Result<FppMonitorResource> fppResult = await _fppdService.ExecuteAsync(resource);
            if (fppResult.Failed)
            {
                fppResult.Errors.ToList().ForEach(e => _logger.LogError(e));
            }

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}
