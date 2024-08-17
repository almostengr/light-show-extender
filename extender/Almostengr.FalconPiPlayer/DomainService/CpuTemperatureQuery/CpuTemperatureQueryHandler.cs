using System.Text;
using Almostengr.LightShowExtender.DomainService.Common;
using Almostengr.Extensions;
using System.Linq;

namespace Almostengr.FalconPiPlayer.DomainService.FalconPiPlayer.CpuTemperatureQueryHandler;

public sealed class CpuTemperatureQueryHandler : IQueryHandler<string>
{
    private const string RASPBERRY_PI = "RASPBERRY PI";
    private const string CPU = "CPU";
    private readonly IFppHttpClient _fppHttpClient;

    public CpuTemperatureQueryHandler(IFppHttpClient fppHttpClient)
    {
        _fppHttpClient = fppHttpClient;
    }

    public async Task<string> ExecuteAsync(CancellationToken cancellationToken)
    {
        var systemsHandler = new MultiSyncSystemsQueryHandler(_fppHttpClient);
        var result = await systemsHandler.ExecuteAsync(cancellationToken, FppSystemType.All);

        var fppSystems = result.Where(s => s.Type.ToUpper().StartsWith(RASPBERRY_PI))
            .Select(s => s.Address)
            .ToList();

        StringBuilder output = new();
        foreach (var system in fppSystems)
        {
            FppStatusRequest request = new(system);
            var response = await systemsHandler.ExecuteAsync(cancellationToken, request);

            var temp = (float)response.Sensors.Where(s => s.Label.StartsWith(CPU))
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
}
