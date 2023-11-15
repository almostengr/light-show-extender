using System.Text;
using Almostengr.LightShowExtender.DomainService.Common;
using Almostengr.Extensions.Logging;

namespace Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

public sealed class GetCpuTemperaturesHandler
{
    private readonly IFppHttpClient _fppHttpClient;
    private readonly GetMultiSyncSystemsHandler _getMultiSyncSystemsHandler;
    private readonly GetStatusHandler _getStatusHandler;
    private readonly ILoggingService<GetCpuTemperaturesHandler> _loggingService;

    public GetCpuTemperaturesHandler(
        ILoggingService<GetCpuTemperaturesHandler> loggingService,
        IFppHttpClient fppHttpClient,
        GetMultiSyncSystemsHandler getMultiSyncSystemsHandler,
        GetStatusHandler getStatusHandler
        )
    {
        _loggingService = loggingService;
        _fppHttpClient = fppHttpClient;
        _getMultiSyncSystemsHandler = getMultiSyncSystemsHandler;
        _getStatusHandler = getStatusHandler;
    }

    public async Task<string> Handle(CancellationToken cancellationToken)
    {
        try
        {
            var multiSyncSystems = await _getMultiSyncSystemsHandler.Handle(cancellationToken);

            var fppSystems = multiSyncSystems.Where(s => s.Type.StartsWith("Raspberry Pi"))
                .Select(s => s.Address)
                .ToList();

            StringBuilder output = new();
            foreach (var system in fppSystems)
            {
                var response = await _getStatusHandler.Handle(cancellationToken, system);

                var temp = (float)response.Sensors.Where(s => s.Label.StartsWith("CPU"))
                    .Select(s => s.Value)
                    .Single();

                if (output.Length > 0)
                {
                    output.Append(", ");
                }

                output.Append(temp.ToDisplayTemperature());
            }

            return output.ToString();
        }
        catch (Exception ex)
        {
            _loggingService.Error(ex, ex.Message);
            return null;
        }
    }
}
