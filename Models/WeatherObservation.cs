using System;

namespace Almostengr.FalconPiMonitor.Models
{
    public class WeatherObservation
    {
        public Properties Properties { get; set; }
    }

    public class Properties
    {
        public string TextDescription { get; set; }
        public DateTime TimeStamp { get; set; }
        public Temperature Temperature { get; set; }
        public HeatIndex HeatIndex { get; set; }
        public WindGust WindGust { get; set; }
        public WindSpeed WindSpeed { get; set; }
    }

    public class Temperature
    {
        public double Value { get; set; }
        public string UnitCode { get; set; }
    }

    public class WindSpeed
    {
        public double Value { get; set; }
        public string UnitCode { get; set; }
    }

    public class WindGust
    {
        public double Value { get; set; }
        public string UnitCode { get; set; }
    }

    public class HeatIndex
    {
        public double Value { get; set; }
        public string UnitCode { get; set; }
    }
}