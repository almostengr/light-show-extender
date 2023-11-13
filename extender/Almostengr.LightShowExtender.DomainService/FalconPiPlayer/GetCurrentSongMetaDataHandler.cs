using Almostengr.Extensions;

namespace Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

public static class GetCurrentSongMetaDataHandler
{
    public static async Task<FppMediaMetaResponse> Handle(IFppHttpClient fppHttpClient, string currentSong, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(currentSong))
        {
            return new();
        }

        return await fppHttpClient.GetCurrentSongMetaDataAsync(currentSong, cancellationToken);
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