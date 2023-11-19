using Almostengr.Extensions.Logging;
using Almostengr.Extensions;

namespace Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

public sealed class InsertPlaylistAfterCurrentHandler : IQueryHandler<InsertPlaylistAfterCurrentRequest, InsertPlaylistAfterCurrentResponse>
{
    private readonly IFppHttpClient _fppHttpClient;
    private readonly ILoggingService<InsertPlaylistAfterCurrentHandler> _loggingService;

    public InsertPlaylistAfterCurrentHandler(
        IFppHttpClient fppHttpClient,
        ILoggingService<InsertPlaylistAfterCurrentHandler> loggingservice)
    {
        _fppHttpClient = fppHttpClient;
        _loggingService = loggingservice;
    }

    public async Task<InsertPlaylistAfterCurrentResponse> ExecuteAsync(InsertPlaylistAfterCurrentRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.PlaylistName == null)
            {
                throw new ArgumentNullException(nameof(request.PlaylistName));
            }

            _loggingService.Information($"Inserted request {request.PlaylistName}");
            var response = await _fppHttpClient.GetInsertPlaylistAfterCurrent(request.PlaylistName, cancellationToken);

            if (response.ToUpper() != "PLAYLIST INSERTED")
            {
                throw new InvalidDataException($"Unexpected response from FPP. {response}");
            }

            return new InsertPlaylistAfterCurrentResponse(true);
        }
        catch (Exception ex)
        {
            _loggingService.Error(ex, ex.Message);
        }

        return new InsertPlaylistAfterCurrentResponse(false);
    }
}

public sealed class InsertPlaylistAfterCurrentRequest : IQueryRequest
{
    public InsertPlaylistAfterCurrentRequest(string playlistName)
    {
        PlaylistName = playlistName;
    }

    public string PlaylistName { get; init; } = string.Empty;
}

public sealed class InsertPlaylistAfterCurrentResponse : IQueryResponse
{
    public InsertPlaylistAfterCurrentResponse(bool succeeded)
    {
        Succeeded = succeeded;
    }

    public bool Succeeded { get; init; } = false;
}
