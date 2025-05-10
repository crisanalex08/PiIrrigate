namespace PiIrrigateServer.Models
{
    public class SensorReading
    {
        public Guid ZoneId { get; set; } // Unique identifier for the zone
        public string Mac { get; set; } // Unique identifier for the device
        public DateTime Timestamp { get; set; } // Timestamp of the reading
        public double Temperature { get; set; } // Temperature
        public double Humidity { get; set; } // Humidity
        public double SoilMoisture { get; set; } // Soil moisture
        public double Rainfall { get; set; } // Rainfall

        public override string ToString()
        {
            return $"ZoneId: {ZoneId}, Mac: {Mac}, Timestamp: {Timestamp}, " +
                   $"Temperature: {Temperature}°C, Humidity: {Humidity}%, " +
                   $"SoilMoisture: {SoilMoisture}%, Rainfall: {Rainfall}mm";
        }
    }
}
