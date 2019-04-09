using System;
using System.Collections.Generic;
using System.Text;

namespace YC.ConsoleApp.Models
{
    public class WaterLevelRealTime
    {
        public List<WaterLevelRecord> Value { get; set; }
    }
    public class WaterLevelRecord
    {
        public string StationIdentifier { get; set; }
        public DateTime RecordTime { get; set; }
        public double WaterLevel { get; set; }
    }
}
