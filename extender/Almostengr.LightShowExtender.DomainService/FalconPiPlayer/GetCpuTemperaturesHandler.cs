using System.Text;
using Almostengr.LightShowExtender.DomainService.Common;
using Almostengr.Common.Logging;
using Microsoft.Extensions.Logging;

namespace Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

public sealed class GetCpuTemperaturesHandler
{
    private readonly IFppHttpClient _fppHttpClient;
    private readonly ILogger<GetCpuTemperaturesHandler> _logger;

    public GetCpuTemperaturesHandler(ILoggingService<GetCpuTemperaturesHandler> logger, IFppHttpClient fppHttpClient)
    {
        _logger = logger;
        _fppHttpClient = fppHttpClient;
    }

    public static async Task<string> Handle(CancellationToken cancellationToken)
    {
        try
        {
            var multiSyncSystems = await GetMultiSyncSystemsHandler.Handle(cancellationToken);

            var fppSystems = multiSyncSystems.Where(s => s.Type.StartsWith("Raspberry Pi"))
                .Select(s => s.Address)
                .ToList();

            StringBuilder output = new();
            foreach (var system in fppSystems)
            {
                var response = await GetStatusHandler.Handle(cancellationToken, system);

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
            _logging.Error(ex.Message);
        }
    }
}
