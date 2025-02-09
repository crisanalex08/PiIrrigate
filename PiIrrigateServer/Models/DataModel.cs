namespace PiIrrigateServer.Models
{
    public class DataModel
    {
        public int IrrigationZoneId;
        public int SensorId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Value { get; set; }
    }
}
