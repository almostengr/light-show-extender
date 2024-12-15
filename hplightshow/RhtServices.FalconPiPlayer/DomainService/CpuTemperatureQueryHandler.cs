using System.Text;
using RhtServices.Common.Query;
using RhtServices.Common;

namespace RhtServices.FalconPiPlayer.DomainService;

public sealed class CpuTemperatureQueryHandler : IQueryHandler<string>
{
    private readonly IFppHttpClient _fppHttpClient;

    public CpuTemperatureQueryHandler(IFppHttpClient fppHttpClient)
    {
        _fppHttpClient = fppHttpClient;
    }

    public async Task<string> ExecuteAsync(CancellationToken cancellationToken)
    {
        var systemsHandler = new MultiSyncSystemsQueryHandler(_fppHttpClient);
        var result = await systemsHandler.ExecuteAsync(cancellationToken, MultiSyncSystemsType.All);

        const string RASPBERRY_PI = "RASPBERRY PI";
        var fppSystems = result.Where(s => s.Type.ToUpper().StartsWith(RASPBERRY_PI))
            .Select(s => s.Address)
            .ToList();

        const string CPU = "CPU";
        StringBuilder output = new();
        var statusHandler = new FppStatusQueryHandler(_fppHttpClient);
        foreach (var system in fppSystems)
        {
            FppStatusQuery request = new(system);
            var response = await statusHandler.ExecuteAsync(cancellationToken, request);

            var temp = (float)response.Sensors.Where(s => s.Label.StartsWith(CPU))
                .Select(s => s.Value)
                .Single();

            if (output.Length > 0)
            {
                output.Append(", ");
            }

            output.Append(temp.ToFahrenheitFromCelsius());
        }

        return output.ToString();
    }
}
