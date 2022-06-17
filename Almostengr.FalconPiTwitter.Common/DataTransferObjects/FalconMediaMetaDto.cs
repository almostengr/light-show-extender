namespace Almostengr.FalconPiTwitter.Common.DataTransferObjects
{
    public class FalconMediaMetaDto : BaseDto
    {
        public FalconMediaMetaFormat Format { get; init; }
    }

    public class FalconMediaMetaFormat
    {
        public FalconMediaMetaFormatTags Tags { get; init; }
    }

    public class FalconMediaMetaFormatTags
    {
        public string Title { get; init; }
        public string Artist { get; init; }
        public string Album { get; init; }
    }
}