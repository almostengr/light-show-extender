namespace Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

public static class GetSequenceList
{
    public static async Task<List<string>> Handle(IFppHttpClient fppHttpClient, CancellationToken cancellationToken)
    {
        return await fppHttpClient.GetSequenceListAsync(cancellationToken);
    }
}
