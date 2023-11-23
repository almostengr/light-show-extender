using Almostengr.Extensions;
using Almostengr.Extensions.Logging;

namespace Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

public sealed class GetSequenceListHandler : IQueryHandler<List<string>>
{
    private readonly IFppHttpClient _fppHttpClient;
    private readonly ILoggingService<GetSequenceListHandler> _loggingService;

    public GetSequenceListHandler(
        IFppHttpClient fppHttpClient,
        ILoggingService<GetSequenceListHandler> loggingservice)
    {
        _fppHttpClient = fppHttpClient;
        _loggingService = loggingservice;
    }

    public async Task<List<string>> ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            return await _fppHttpClient.GetSequenceListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _loggingService.Error(ex, ex.Message);
            return null!;
        }
    }
}
