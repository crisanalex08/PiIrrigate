#pragma once

struct SensorData {
    float temperature;   // raw value
    float humidity;      // 
    float soilMoisture;  //
    float rainLevel;     //
    float waterTemp;     // water temperature
    float totalWaterFlow; // total water flow
    float longitude;     // GPS longitude
    float latitude;      // GPS latitude

    SensorData()
        : temperature(0.0f), humidity(0.0f), soilMoisture(0.0f), rainLevel(0.0f),
          waterTemp(0.0f), totalWaterFlow(0.0f), longitude(0.0f), latitude(0.0f) {}
};

//Struct for LoRa packet
struct LoRaPacket {
    int packetCount;      // Packet count for tracking
    SensorData sensorData; // Sensor data to be sent
    int stationMac[6]; // MAC address of the device

    LoRaPacket()
        : packetCount(0), stationMac{0} {}
};

