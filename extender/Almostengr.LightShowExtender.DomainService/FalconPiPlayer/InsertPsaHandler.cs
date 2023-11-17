using Almostengr.Extensions.Logging;
using Almostengr.Extensions;

namespace Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

public sealed class InsertPsaHandler : ICommandHandler<string>
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

    public async Task ExecuteAsync(string sequenceFilter, CancellationToken cancellationToken)
    {
        try
        {
            var allSequences = await _getSequenceListHandler.ExecuteAsync(cancellationToken);
            string psaSequence = allSequences.Where(s => s.ToUpper().Contains("PSA")).First();
            psaSequence = psaSequence.Contains(".fseq") ? psaSequence : $"{psaSequence}.fseq";
            await _insertPlaylistAfterCurrentHandler.ExecuteAsync(psaSequence, cancellationToken);
        }
        catch (Exception ex)
        {
            _loggingService.Error(ex, ex.Message);
        }
    }
}
