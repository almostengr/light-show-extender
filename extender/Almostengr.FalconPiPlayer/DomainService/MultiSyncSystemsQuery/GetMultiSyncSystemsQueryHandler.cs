using Almostengr.Extensions;

namespace Almostengr.FalconPiPlayer.DomainService.FalconPiPlayer;

public sealed class MultiSyncSystemsQueryHandler : IQueryHandler<FppSystemType, List<FppMultiSyncSystemsResponse.FppSystem>>
{
    private readonly IFppHttpClient _fppHttpClient;

    public MultiSyncSystemsQueryHandler(IFppHttpClient fppHttpClient)
    {
        _fppHttpClient = fppHttpClient;
    }

    public async Task<List<FppMultiSyncSystemsResponse.FppSystem>> ExecuteAsync(CancellationToken cancellationToken, FppSystemType type)
    {
        var systems = await _fppHttpClient.GetMultiSyncSystemsAsync(cancellationToken);

        if (type == FppSystemType.All)
        {
            return systems.Systems;
        }

        return systems.Systems
            .Where(s => s.Type.ToUpper() == type.Value)
            .ToList();
    }
}
