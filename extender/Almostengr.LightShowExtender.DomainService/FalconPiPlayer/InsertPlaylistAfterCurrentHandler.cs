namespace Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

public sealed class InsertPlaylistAfterCurrentHandler
{
    private readonly IFppHttpClient _fppHttpClient;

    public InsertPlaylistAfterCurrentHandler(IFppHttpClient fppHttpClient)
    {
        _fppHttpClient = fppHttpClient;
    }

    public async Task Handle(string playlistName, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(playlistName))
        {
            throw new ArgumentNullException(nameof(playlistName));
        }

        var response = await _fppHttpClient.GetInsertPlaylistAfterCurrent(playlistName, cancellationToken);

        if (response.ToUpper() != "PLAYLIST INSERTED")
        {
            throw new InvalidDataException($"Unexpected response from FPP. {response}");
        }
    }
}