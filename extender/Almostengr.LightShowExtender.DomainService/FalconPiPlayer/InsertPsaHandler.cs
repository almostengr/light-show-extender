namespace Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

public sealed class InsertPsaHandler
{
    private readonly IFppHttpClient _fppHttpClient;

    public InsertPsaHandler(IFppHttpClient fppHttpClient)
    {
        _fppHttpClient = fppHttpClient;
    }

    public async Task Handle(CancellationToken cancellationToken)
    {
        var allSequences = await GetSequenceListHandler.Handle(cancellationToken);
        string psaSequence = allSequences.Where(s => s.ToUpper().Contains("PSA")).First();
        psaSequence = psaSequence.Contains(".fseq") ? psaSequence : $"{psaSequence}.fseq";
        await InsertPlaylistAfterCurrentHandler.Handle(psaSequence, cancellationToken);
    }
}
