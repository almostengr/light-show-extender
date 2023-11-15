using Almostengr.Extensions.Logging;

namespace Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

public sealed class StopShowAfterEndTimeHandler
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

    public async Task Handle(string currentPlaylist, CancellationToken cancellationToken)
    {
        try
        {
            var showEndTime = new TimeSpan(22, 15, 00);
            if (currentPlaylist.ToUpper().Contains("CHRISTMAS") && DateTime.Now.TimeOfDay >= showEndTime)
            {
                await _fppHttpClient.StopPlaylistGracefullyAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _loggingService.Error(ex, ex.Message);
        }
    }
}