using Almostengr.Extensions;
using Almostengr.Extensions.Logging;

namespace Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

public sealed class GetCurrentSongMetaDataHandler : IQueryHandler<FppMediaMetaRequest, FppMediaMetaResponse>
{
    private readonly IFppHttpClient _fppHttpClient;
    private readonly ILoggingService<GetCurrentSongMetaDataHandler> _loggingService;

    public GetCurrentSongMetaDataHandler(
        IFppHttpClient fppHttpClient,
        ILoggingService<GetCurrentSongMetaDataHandler> loggingService)
    {
        _fppHttpClient = fppHttpClient;
        _loggingService = loggingService;
    }

    public async Task<FppMediaMetaResponse> ExecuteAsync(FppMediaMetaRequest request, CancellationToken cancellationToken)
    {
        try
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
        catch (Exception ex)
        {
            _loggingService.Error(ex, ex.Message);
            return null!;
        }
    }
}

public sealed class FppMediaMetaResponse : IQueryResponse
{
    public FalconMediaMetaFormat Format { get; init; } = new();

    public sealed class FalconMediaMetaFormat
    {
        public FalconMediaMetaFormatTags Tags { get; init; } = new();

        public sealed class FalconMediaMetaFormatTags
        {
            public string Title { get; init; } = string.Empty;
            public string Artist { get; init; } = string.Empty;
        }
    }
}

public sealed class FppMediaMetaRequest : IQueryRequest
{
    public FppMediaMetaRequest(string currentSong)
    {
        CurrentSong = currentSong;
    }

    public string CurrentSong { get; init; }
}