using Almostengr.Extensions;

namespace Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

public static class GetMultiSyncSystemsHandler
{
    public static async Task<List<FppMultiSyncSystemsResponse.FppSystem>> Handle(IFppHttpClient fppHttpClient, CancellationToken cancellationToken, string type = "")
    {
        var systems = await fppHttpClient.GetMultiSyncSystemsAsync(cancellationToken);
        
        if (string.IsNullOrWhiteSpace(type))
        {
            return systems.Systems;
        }

        return systems.Systems
            .Where(s => s.Type.ToUpper() == type)
            .ToList();
    }
}

public sealed class FppMultiSyncSystemsResponse : BaseResponse
{
    public List<FppSystem> Systems { get; init; } = new();

    public sealed class FppSystem
    {
        public string Address { get; init; } = string.Empty;
        public string Hostname { get; init; } = string.Empty;
        public string Type { get; init; } = string.Empty;
    }
}

public sealed class FppSystemType
{
    public FppSystemType(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static readonly FppSystemType RaspberryPi3 = new FppSystemType("Raspberry Pi 3 B");
    public static readonly FppSystemType Unknown = new FppSystemType("Unknown");
    public static readonly FppSystemType UnknownSystem = new FppSystemType("Unknown System Type");
    public static readonly FppSystemType WLED = new FppSystemType("WLED");
}