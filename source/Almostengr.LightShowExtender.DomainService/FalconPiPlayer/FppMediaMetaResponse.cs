using Almostengr.Extensions;

namespace Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

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