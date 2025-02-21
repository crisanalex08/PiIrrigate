using PiIrrigateServer.Models;

namespace PiIrrigateServer.Entities
{
    public class DataEntity
    {
        public int Id { get; set; }
        public int IrrigationZoneId { get; set; } 
        public int SensorId { get; set; }
        public DateTime Timestamp { get; set; }
        public DataModel Value { get; set; }
    }
}