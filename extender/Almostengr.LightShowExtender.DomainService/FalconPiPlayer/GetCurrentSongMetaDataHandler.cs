using Almostengr.Extensions;

namespace Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

public sealed class GetCurrentSongMetaDataHandler
{
    private readonly IFppHttpClient _fppHttpClient;

    public GetCurrentSongMetaDataHandler(IFppHttpClient fppHttpClient)
    {
        _fppHttpClient = fppHttpClient;
    }

    public async Task<FppMediaMetaResponse> Handle(string currentSong, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(currentSong))
            {
                return new();
            }

            return await _fppHttpClient.GetCurrentSongMetaDataAsync(currentSong, cancellationToken);
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}

public sealed class FppMediaMetaResponse : BaseResponse
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