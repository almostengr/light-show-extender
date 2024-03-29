using Almostengr.Extensions;
using Almostengr.Extensions.Logging;

namespace Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

public sealed class GetMultiSyncSystemsHandler : IQueryHandler<string, List<FppMultiSyncSystemsResponse.FppSystem>>
{
    private readonly IFppHttpClient _fppHttpClient;
    private readonly ILoggingService<GetMultiSyncSystemsHandler> _loggingService;

    public GetMultiSyncSystemsHandler(
        IFppHttpClient fppHttpClient,
        ILoggingService<GetMultiSyncSystemsHandler> loggingService)
    {
        _fppHttpClient = fppHttpClient;
        _loggingService = loggingService;
    }

    public async Task<List<FppMultiSyncSystemsResponse.FppSystem>> ExecuteAsync(string type, CancellationToken cancellationToken)
    {
        try
        {
            var systems = await _fppHttpClient.GetMultiSyncSystemsAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(type))
            {
                return systems.Systems;
            }

            return systems.Systems
                .Where(s => s.Type.ToUpper() == type)
                .ToList();
        }
        catch (Exception ex)
        {
            _loggingService.Error(ex, ex.Message);
            return null!;
        }
    }
}

public sealed class FppMultiSyncSystemsResponse : IQueryResponse
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