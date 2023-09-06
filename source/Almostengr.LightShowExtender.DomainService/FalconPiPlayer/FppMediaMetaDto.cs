using Almostengr.LightShowExtender.DomainService.Common;

namespace Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

public class FppMediaMetaDto : BaseDto
{
    public FalconMediaMetaFormat Format { get; init; } = new();

    public class FalconMediaMetaFormat
    {
        public FalconMediaMetaFormatTags Tags { get; init; } = new();

        public class FalconMediaMetaFormatTags
        {
            public string Title { get; init; } = string.Empty;
            public string Artist { get; init; } = string.Empty;
            public string Album { get; init; } = string.Empty;
        }
    }
}