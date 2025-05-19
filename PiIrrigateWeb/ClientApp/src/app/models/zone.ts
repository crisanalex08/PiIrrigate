export interface Zone {
  zoneId: string;                // Unique identifier for the zone (GUID as string)
  name?: string;                 // Name of the zone (optional)
  userId?: string;               // User ID (optional, GUID as string)
}

export interface SensorReading {
  timestamp: Date;         // Timestamp of the reading
  zoneId: string;          // Unique identifier for the zone (GUID as string)
  mac: string;             // Unique identifier for the device
  temperature: number;     // Temperature
  humidity: number;        // Humidity
  soilMoisture: number;    // Soil moisture
  rainfall: number;        // Rainfall
}

export interface Device {
  mac: string;             // Unique identifier for the device (MAC address)
  zoneId: string;          // Unique identifier for the zone (GUID as string)
  name?: string;           // Name of the device (optional)
  location?: string;     // Location of the device (optional)
}