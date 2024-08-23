using Almostengr.Common.Query;

namespace Almostengr.FalconPiPlayer.DomainService;

public sealed class FppStatusQueryHandler : IQueryHandler<FppStatusRequest, FppStatusResponse>
{
    private readonly IFppHttpClient _fppHttpClient;

    public FppStatusQueryHandler(IFppHttpClient fppHttpClient)
    {
        _fppHttpClient = fppHttpClient;
    }

    public async Task<FppStatusResponse> ExecuteAsync(CancellationToken cancellationToken, FppStatusRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        return await _fppHttpClient.GetFppdStatusAsync(cancellationToken, request.Hostname);
    }
}