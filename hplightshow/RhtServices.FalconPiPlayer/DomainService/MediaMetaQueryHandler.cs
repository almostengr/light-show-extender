using RhtServices.Common.Query;

namespace RhtServices.FalconPiPlayer.DomainService;

public sealed class MediaMetaQueryHandler : IQueryHandler<MediaMetaRequest, MediaMetaResponse>
{
    private readonly IFppHttpClient _fppHttpClient;

    public MediaMetaQueryHandler(IFppHttpClient fppHttpClient)
    {
        _fppHttpClient = fppHttpClient;
    }

    public async Task<MediaMetaResponse> ExecuteAsync(CancellationToken cancellationToken, MediaMetaRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (string.IsNullOrWhiteSpace(request.CurrentSong))
        {
            return new();
        }

        return await _fppHttpClient.GetCurrentSongMetaDataAsync(request.CurrentSong, cancellationToken);
    }
}
