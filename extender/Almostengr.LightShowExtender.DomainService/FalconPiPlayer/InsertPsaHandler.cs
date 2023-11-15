using Almostengr.Extensions.Logging;

namespace Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

public sealed class InsertPsaHandler
{
    private readonly IFppHttpClient _fppHttpClient;
    private readonly GetSequenceListHandler _getSequenceListHandler;
    private readonly InsertPlaylistAfterCurrentHandler _insertPlaylistAfterCurrentHandler;
    private readonly ILoggingService<InsertPsaHandler> _loggingService;

    public InsertPsaHandler(
        IFppHttpClient fppHttpClient,
        GetSequenceListHandler getSequenceListHandler,
        InsertPlaylistAfterCurrentHandler insertPlaylistAfterCurrentHandler,
        ILoggingService<InsertPsaHandler> loggingService
        )
    {
        _fppHttpClient = fppHttpClient;
        _getSequenceListHandler = getSequenceListHandler;
        _insertPlaylistAfterCurrentHandler = insertPlaylistAfterCurrentHandler;
        _loggingService = loggingService;
    }

    public async Task Handle(CancellationToken cancellationToken)
    {
        try {
        var allSequences = await _getSequenceListHandler.Handle(cancellationToken);
        string psaSequence = allSequences.Where(s => s.ToUpper().Contains("PSA")).First();
        psaSequence = psaSequence.Contains(".fseq") ? psaSequence : $"{psaSequence}.fseq";
        await _insertPlaylistAfterCurrentHandler.Handle(psaSequence, cancellationToken);
        }
        catch (Exception ex)
        {
            _loggingService.Error(ex, ex.Message);
        }
    }
}
