using System.Collections.Generic;

namespace Almostengr.FalconPiMonitor
{
    public class FalconStatus
    {
        public FalconStatusCurrentPlayList Current_PlayList { get; set; }
        public string Current_Song { get; set; }

        private string _currentSequence;
        public string Current_Sequence
        {
            get { return _currentSequence; }
            set { _currentSequence = value.Replace(".fseq", ""); }
        }

        public string Fppd { get; set; }
        public string Status_Name { get; set; }
        public IList<FalconStatusSensor> Sensors { get; set; }
    }

    public class FalconStatusSensor
    {
        public string Formatted { get; set; }
        public string Label { get; set; }
        public double Value { get; set; }
        public string ValueType { get; set; }

        public double DegreesCToF()
        {
            return (Value * 1.8) + 32;
        }
    }

    public class FalconStatusCurrentPlayList
    {
        public int Count { get; set; }
        public string Description { get; set; }
        public int Index { get; set; }
        public string Playlist { get; set; }
        public string Type { get; set; }
    }

    public class FalconStatusMediaMeta
    {
        public FalconStatusMediaMetaFormat Format { get; set; }
    }

    public class FalconStatusMediaMetaFormat
    {
        public FalconStatusMediaMetaFormatTags Tags { get; set; }
    }

    public class FalconStatusMediaMetaFormatTags
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Date { get; set; }
    }
}