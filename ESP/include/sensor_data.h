#pragma once

#include <string>
#include <vector>

// Struct for sensor data
// ALl values are float to allow for decimal precision
// All readings represent physical quantities that can be measured by sensors.
struct SensorData {
    float temperature;
    float humidity;
    float soilMoisture;
    float rainLevel;
    float waterTemp;
    float totalWaterFlow;
    float longitude;
    float latitude;

    SensorData()
        : temperature(0.0f), humidity(0.0f), soilMoisture(0.0f), rainLevel(0.0f),
          waterTemp(0.0f), totalWaterFlow(0.0f), longitude(0.0f), latitude(0.0f) {}
};

//Struct for LoRa packet
#pragma pack(push, 1)
struct LoRaPacket {
    int packetCount;      // Packet count for tracking
    SensorData sensorData; // Sensor data to be sent
    int stationMac[6]; // MAC address of the device

    LoRaPacket()
        : packetCount(0), stationMac{0} {}
};
#pragma pack(pop)

struct LoRaCommandParameter {
    std::string parameterName; // Name of the parameter
    std::string parameterValue; // Value of the parameter
};

struct LoRaCommand {
    int stationMac[6]; // MAC address of the device
    std::string methodName; // Command string to be sent
    std::vector<LoRaCommandParameter> parameters; // List of parameters for the command
};

