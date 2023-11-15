using Almostengr.Extensions.Logging;

namespace Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

public sealed class InsertPlaylistAfterCurrentHandler
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

    public async Task Handle(string playlistName, CancellationToken cancellationToken)
    {
        try
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
        catch (Exception ex)
        {
            _loggingService.Error(ex, ex.Message);
        }
    }
}