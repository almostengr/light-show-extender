using RhtServices.Common.Query;

namespace RhtServices.FalconPiPlayer.DomainService;

public sealed class MultiSyncSystemsQueryHandler : IQueryHandler<MultiSyncSystemsType, List<MultiSyncSystemsQueryResponse.FppSystem>>
{
    private readonly IFppHttpClient _fppHttpClient;

    public MultiSyncSystemsQueryHandler(IFppHttpClient fppHttpClient)
    {
        _fppHttpClient = fppHttpClient;
    }

    public async Task<List<MultiSyncSystemsQueryResponse.FppSystem>> ExecuteAsync(CancellationToken cancellationToken, MultiSyncSystemsType type)
    {
        var systems = await _fppHttpClient.GetMultiSyncSystemsAsync(cancellationToken);

        if (type == MultiSyncSystemsType.All)
        {
            return systems.Systems;
        }

        return systems.Systems
            .Where(s => s.Type.ToUpper() == type.Value)
            .ToList();
    }
}
