using System.Text;
using RhtServices.Common.Utilities.DomainService;
using RhtServices.Common.Utilities.Shared;

namespace RhtServices.FalconPiPlayer.DomainService;

public sealed class CpuTemperatureQueryHandler : IHandler<string>
{
    private readonly IFppHttpClient _fppHttpClient;

    public CpuTemperatureQueryHandler(IFppHttpClient fppHttpClient)
    {
        _fppHttpClient = fppHttpClient;
    }

    public async Task<string> ExecuteAsync()
    {
        var systemsHandler = new MultiSyncSystemsQueryHandler(_fppHttpClient);
        var result = await systemsHandler.ExecuteAsync( MultiSyncSystemsType.All);

        const string RASPBERRY_PI = "RASPBERRY PI";
        var fppSystems = result.Where(s => s.Type.ToUpper().StartsWith(RASPBERRY_PI))
            .Select(s => s.Address)
            .ToList();

        const string CPU = "CPU";
        StringBuilder output = new();
        foreach (var system in fppSystems)
        {
            var response = await _fppHttpClient.GetFppdStatusAsync();

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
