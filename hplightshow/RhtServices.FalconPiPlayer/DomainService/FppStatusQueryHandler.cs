using RhtServices.Common.Query;

namespace RhtServices.FalconPiPlayer.DomainService;

public sealed class FppStatusQueryHandler : IQueryHandler<FppStatusQuery, FppStatusResponse>
{
    private readonly IFppHttpClient _fppHttpClient;

    public FppStatusQueryHandler(IFppHttpClient fppHttpClient)
    {
        _fppHttpClient = fppHttpClient;
    }

    public async Task<FppStatusResponse> ExecuteAsync(CancellationToken cancellationToken, FppStatusQuery request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        return await _fppHttpClient.GetFppdStatusAsync(cancellationToken, request.Hostname);
    }
}
