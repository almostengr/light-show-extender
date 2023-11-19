using Almostengr.Extensions.Logging;
using Almostengr.Extensions;

namespace Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

public sealed class InsertPsaHandler : IQueryHandler<InsertPlaylistAfterCurrentResponse>
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

    public async Task<InsertPlaylistAfterCurrentResponse> ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            var allSequences = await _getSequenceListHandler.ExecuteAsync(cancellationToken);
            string psaSequence = allSequences.Where(s => s.ToUpper().Contains("PSA")).First();
            psaSequence = psaSequence.Contains(".fseq") ? psaSequence : $"{psaSequence}.fseq";

            InsertPlaylistAfterCurrentRequest request = new(psaSequence);
            return await _insertPlaylistAfterCurrentHandler.ExecuteAsync(request, cancellationToken);
        }
        catch (Exception ex)
        {
            _loggingService.Error(ex, ex.Message);
        }

        return new InsertPlaylistAfterCurrentResponse(false);
    }
}
