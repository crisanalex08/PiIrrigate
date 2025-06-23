using System.ComponentModel.DataAnnotations;

namespace PiIrrigateServer.Models
{
    public class SensorReading
    {
        //C:16, ID:CEE0F9088C7B00000000, T:26.00, H:34.00, S:142.00, R:3134.00, WT:0.00, WF:0.00, Lon:0.000000, Lat:0.000000, CT:1750275804, W:1
        [Key]
        public DateTime Timestamp { get; set; } // Timestamp of the reading
        public Guid ZoneId { get; set; } // Unique identifier for the zone
        public string Mac { get; set; } // Unique identifier for the device
        public double Temperature { get; set; } // Temperature
        public double Humidity { get; set; } // Humidity
        public double SoilMoisture { get; set; } // Soil moisture
        public double Rainfall { get; set; } // Rainfall
        public double WaterFlow { get; set; } // Water flow
        public double WaterTemperature { get; set; } // Water temperature
        public double Longitude { get; set; } // Longitude of the sensor location
        public double Latitude { get; set; } // Latitude of the sensor location
        public DateTime ReadingTime { get; set; } // Time when the reading was taken
        public bool Watering { get; set; } // Indicates if watering is currently active

        public override string ToString()
        {
            return $"Sensor Reading: [Timestamp: {Timestamp}, ZoneId: {ZoneId}, Mac: {Mac}, " +
                   $"Temperature: {Temperature}°C, Humidity: {Humidity}%, SoilMoisture: {SoilMoisture}%, " +
                   $"Rainfall: {Rainfall}mm, WaterFlow: {WaterFlow}L/min, WaterTemperature: {WaterTemperature}°C, " +
                   $"Longitude: {Longitude}, Latitude: {Latitude}, ReadingTime:{ReadingTime}, Watering:{Watering}]";
        }
    }
}
