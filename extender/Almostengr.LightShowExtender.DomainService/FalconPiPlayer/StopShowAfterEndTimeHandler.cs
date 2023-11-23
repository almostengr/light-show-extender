using Almostengr.Extensions.Logging;
using Almostengr.Extensions;

namespace Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

public sealed class StopShowAfterEndTimeHandler : IQueryHandler<StopShowAfterEndTimeRequest, StopShowAfterEndTimeResponse>
{
    private readonly IFppHttpClient _fppHttpClient;
    private readonly ILoggingService<StopShowAfterEndTimeHandler> _loggingService;

    public StopShowAfterEndTimeHandler(
        IFppHttpClient fppHttpClient,
        ILoggingService<StopShowAfterEndTimeHandler> loggingService)
    {
        _fppHttpClient = fppHttpClient;
        _loggingService = loggingService;
    }

    public async Task<StopShowAfterEndTimeResponse> ExecuteAsync(StopShowAfterEndTimeRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.CurrentPlaylist))
            {
                throw new ArgumentNullException(nameof(request.CurrentPlaylist));
            }

            var showEndTime = new TimeSpan(22, 15, 00);
            if (request.CurrentPlaylist.Contains("CHRISTMAS") && DateTime.Now.TimeOfDay >= showEndTime)
            {
                await _fppHttpClient.StopPlaylistGracefullyAsync(cancellationToken);
                _loggingService.Warning("Stopping show after end time");
            }

            return new StopShowAfterEndTimeResponse(true);
        }
        catch (Exception ex)
        {
            _loggingService.Error(ex, ex.Message);
        }

        return new StopShowAfterEndTimeResponse(false);
    }
}

public sealed class StopShowAfterEndTimeRequest : IQueryRequest
{
    public StopShowAfterEndTimeRequest(string currentPlaylist)
    {
        CurrentPlaylist = currentPlaylist.ToUpper();
    }

    public string CurrentPlaylist { get; init; } = string.Empty;
}

public sealed class StopShowAfterEndTimeResponse : IQueryResponse
{
    public StopShowAfterEndTimeResponse(bool succeeded)
    {
        Succeeded = succeeded;
    }

    public bool Succeeded { get; init; } = false;
}