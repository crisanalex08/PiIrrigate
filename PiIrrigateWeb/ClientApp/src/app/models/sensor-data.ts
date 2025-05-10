export interface SensorData {
  zoneId: string;         // Unique identifier for the zone
  mac: string;            // Unique identifier for the device
  timestamp: Date;        // Timestamp of the reading
  temperature: number;    // Temperature
  humidity: number;       // Humidity
  soilMoisture: number;   // Soil moisture
  rainfall: number;       // Rainfall
}