export interface SensorData {
  timestamp: Date;           // Timestamp of the reading
  zoneId: string;            // Unique identifier for the zone (GUID as string)
  mac: string;               // Unique identifier for the device
  temperature: number;       // Temperature
  humidity: number;          // Humidity
  soilMoisture: number;      // Soil moisture
  rainfall: number;          // Rainfall
  waterFlow: number;         // Water flow
  waterTemperature: number;  // Water temperature
  longitude: number;         // Longitude of the sensor location
  latitude: number;          // Latitude of the sensor location
}