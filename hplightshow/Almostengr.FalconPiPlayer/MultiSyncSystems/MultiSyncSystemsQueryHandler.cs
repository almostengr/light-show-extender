using Almostengr.Common.Utilities.DomainService;

namespace Almostengr.FalconPiPlayer.DomainService;

public sealed class MultiSyncSystemsQueryHandler : IHandler<MultiSyncSystemsType, List<MultiSyncSystemsQueryResponse.FppSystem>>
{
    private readonly IFppHttpClient _fppHttpClient;

    public MultiSyncSystemsQueryHandler(IFppHttpClient fppHttpClient)
    {
        _fppHttpClient = fppHttpClient;
    }

    public async Task<List<MultiSyncSystemsQueryResponse.FppSystem>> ExecuteAsync( MultiSyncSystemsType type)
    {
        var systems = await _fppHttpClient.GetMultiSyncSystemsAsync();

        if (type == MultiSyncSystemsType.All)
        {
            return systems.Systems;
        }

        return systems.Systems
            .Where(s => s.Type.ToUpper() == type.Value)
            .ToList();
    }
}
