namespace PiIrrigateServer.Entities
{
    public class DataEntity
    {
        public int Id { get; set; }
        public int IrrigationZoneId { get; set; } 
        public int SensorId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Value { get; set; }
    }
}