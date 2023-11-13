namespace Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

public static class StopShowAfterEndTimeHandler
{
    public static async Task Handle(IFppHttpClient fppHttpClient, string currentPlaylist, CancellationToken cancellationToken)
    {
        var showEndTime = new TimeSpan(22, 15, 00);
        if (currentPlaylist.ToUpper().Contains("CHRISTMAS") && DateTime.Now.TimeOfDay >= showEndTime)
        {
            await fppHttpClient.StopPlaylistGracefullyAsync(cancellationToken);
        }
    }
}