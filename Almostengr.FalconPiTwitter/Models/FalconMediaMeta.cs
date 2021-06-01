namespace Almostengr.FalconPiTwitter.Models
{
    public class FalconMediaMeta
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