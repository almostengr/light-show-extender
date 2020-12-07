using System.Collections.Generic;

namespace Almostengr.FalconPiMonitor
{
    public class FalconStatus
    {
        public FalconStatusCurrentPlayList Current_PlayList { get; set; }
        public string Current_Song { get; set; }
        public string Current_Sequence { get; set; }
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
    }

    public class FalconStatusCurrentPlayList
    {
        public int Count { get; set; }
        public string Description { get; set; }
        public int Index { get; set; }
        public string Playlist { get; set; }
        public string Type { get; set; }
    }
}