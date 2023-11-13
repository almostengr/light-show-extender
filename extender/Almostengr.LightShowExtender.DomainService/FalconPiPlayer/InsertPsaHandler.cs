namespace Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

public static class InsertPsaHandler
{
    public static async Task Handle(IFppHttpClient fppHttpClient, CancellationToken cancellationToken)
    {
        var allSequences = await GetSequenceList.Handle(fppHttpClient, cancellationToken);
        string psaSequence = allSequences.Where(s => s.ToUpper().Contains("PSA")).First();
        psaSequence = psaSequence.Contains(".fseq") ? psaSequence : $"{psaSequence}.fseq";
        await InsertPlaylistAfterCurrentHandler.Handle(fppHttpClient, psaSequence, cancellationToken);
    }
}
