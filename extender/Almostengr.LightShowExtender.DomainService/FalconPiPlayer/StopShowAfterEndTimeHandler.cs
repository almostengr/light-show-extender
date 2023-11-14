namespace Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

public sealed class StopShowAfterEndTimeHandler
{
    private readonly IFppHttpClient _fppHttpClient;

    public StopShowAfterEndTimeHandler(IFppHttpClient fppHttpClient)
    {
        _fppHttpClient = fppHttpClient;
    }

    public static async Task Handle(string currentPlaylist, CancellationToken cancellationToken)
    {
        var showEndTime = new TimeSpan(22, 15, 00);
        if (currentPlaylist.ToUpper().Contains("CHRISTMAS") && DateTime.Now.TimeOfDay >= showEndTime)
        {
            await _fppHttpClient.StopPlaylistGracefullyAsync(cancellationToken);
        }
    }
}