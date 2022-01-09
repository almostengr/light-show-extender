namespace Almostengr.FalconPiTwitter.DataTransferObjects
{
    public class FalconMediaMetaDto : BaseDto
    {
        public FalconMediaMetaFormat Format { get; set; }
    }

    public class FalconMediaMetaFormat
    {
        public FalconMediaMetaFormatTags Tags { get; set; }
    }

    public class FalconMediaMetaFormatTags
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
    }
}