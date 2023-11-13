namespace Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

public static class InsertPlaylistAfterCurrentHandler
{
    public static async Task Handle(IFppHttpClient fppHttpClient, string playlistName, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(playlistName))
        {
            throw new ArgumentNullException(nameof(playlistName));
        }

        var response = await fppHttpClient.GetInsertPlaylistAfterCurrent(playlistName, cancellationToken);

        if (response.ToUpper() != "PLAYLIST INSERTED")
        {
            throw new InvalidDataException($"Unexpected response from FPP. {response}");
        }
    }
}