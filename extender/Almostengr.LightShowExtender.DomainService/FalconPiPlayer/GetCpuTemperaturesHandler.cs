using System.Text;
using Almostengr.LightShowExtender.DomainService.Common;
using Microsoft.Extensions.Logging;

namespace Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

public static class GetCpuTemperaturesHandler
{
    public static async Task<string> Handle(IFppHttpClient fppHttpClient, CancellationToken cancellationToken)
    {
        var multiSyncSystems = await GetMultiSyncSystemsHandler.Handle(fppHttpClient, cancellationToken);

        var fppSystems = multiSyncSystems.Where(s => s.Type.StartsWith("Raspberry Pi"))
            .Select(s => s.Address)
            .ToList();

        StringBuilder output = new();
        foreach (var system in fppSystems)
        {
            var response = await GetStatusHandler.Handle(fppHttpClient, cancellationToken, system);
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
}
