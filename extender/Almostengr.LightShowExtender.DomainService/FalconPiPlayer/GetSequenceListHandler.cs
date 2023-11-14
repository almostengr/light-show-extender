namespace Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

public sealed class GetSequenceListHandler
{
    private readonly IFppHttpClient _fppHttpClient;

    public GetSequenceListHandler(IFppHttpClient fppHttpClient)
    {
        _fppHttpClient = fppHttpClient;
    }

    public async Task<List<string>> Handle(CancellationToken cancellationToken)
    {
        try
        {
            return await _fppHttpClient.GetSequenceListAsync(cancellationToken);
        }
        catch (Exception)
        {
            return null;
        }
    }
}
