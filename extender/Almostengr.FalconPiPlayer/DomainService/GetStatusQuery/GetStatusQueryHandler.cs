using Almostengr.Extensions;

namespace Almostengr.FalconPiPlayer.DomainService.FalconPiPlayer;

public sealed class GetStatusQueryHandler : IQueryHandler<FppStatusRequest, FppStatusResponse>
{
    private readonly IFppHttpClient _fppHttpClient;

    public GetStatusQueryHandler(IFppHttpClient fppHttpClient)
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
