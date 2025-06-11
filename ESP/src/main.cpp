#include <Arduino.h>
#include "driver/gpio.h"
#include <dht.h>
#include <rain.h>
#include <soil.h>
#include <serial.h>
#include <LoraSender.h>
#include <LoraReceiver.h>
#include "water_temp.h"
#include "gps.h"
#include "water_flow.h"
// Define GPIO pin for an LED (example: GPIO2)
#define LED_PIN GPIO_NUM_4
#define MESSAGE_TIMEOUT 20000 // 2 seconds timeout for acknowledgment
#define RELAY_PIN GPIO_NUM_22
#define DS18B20_PIN 32
int packetCount = 0; // Packet count for debugging

DHT dht;
RainSensor rainSensor;
SoilSensor soilSensor;
MySerial customSerial;
int newInterval = 10; // Default interval in seconds
WaterFlowSensor flowSensor(GPIO_NUM_32);
GpsModule gpsModule(SERIAL_8N1, GPIO_NUM_34, GPIO_NUM_12, 9600);
WaterTempSensor waterTemp(DS18B20_PIN);

LoraSender loraSender;
LoraReceiver loraReceiver;

void setup()
{
    // Initialize the serial communication
    Serial.begin(115200);
    customSerial.init();

    // Initialize sensors
    dht.init();
    rainSensor.init();
    soilSensor.init();
    waterTemp.init();
    flowSensor.init();

    // Initialize LoRa sender
    loraSender.init();
#
    // Initialize LoRa receiver
    loraReceiver.init();

    // Configure the LED pin as output
    pinMode(LED_PIN, OUTPUT);
    pinMode(RELAY_PIN, OUTPUT);

    Serial.println("System initialized");
}

void loop()
{
#ifdef SENDER
    static unsigned long lastSendTime = 0;
    if (millis() - lastSendTime >= newInterval * 1000)
    {
        lastSendTime = millis();
        int humidity = 0, temperature = 0;
        int soilMoisture = 0, rainLevel = 0;
        int waterFlow = 0, waterTempValue = 0;

        float longitude = 0, latitude = 0;

        flowSensor.update();
        float totalLiters = flowSensor.getTotalLiters();

        // Read data from DHT sensor
        if (dht.read(humidity, temperature) != 0)
        {
            Serial.println("Failed to read from DHT sensor");
        }

        // Read data from RainSensor
        rainLevel = rainSensor.read();

        // Read data from SoilSensor
        soilMoisture = soilSensor.read();

        waterTempValue = waterTemp.readTemperature();

        gpsModule.update(); // Update GPS data
        if (gpsModule.hasFix())
        {
            longitude = gpsModule.getLongitude();
            latitude = gpsModule.getLatitude();
            Serial.printf("GPS Fix: Lat: %.6f, Lon: %.6f\n", latitude, longitude);
        }
        else
        {
            Serial.println("No GPS fix available");
        }

        SensorData sensorData;
        sensorData.temperature = static_cast<float>(temperature);
        sensorData.humidity = static_cast<float>(humidity);
        sensorData.soilMoisture = static_cast<float>(soilMoisture);
        sensorData.rainLevel = static_cast<float>(rainLevel);
        sensorData.waterTemp = waterTempValue;
        sensorData.totalWaterFlow = totalLiters; // Total water flow in liters
        sensorData.longitude = longitude;        // Placeholder for GPS data
        sensorData.latitude = latitude;          // Placeholder for GPS data

        // Send sensor data via custom serial
        customSerial.sendSensorData(packetCount, sensorData);

        // Send sensor data via LoRa
        loraSender.sendSensorDataWithAck(sensorData, packetCount);
        packetCount++; // Increment packet count for the next message
        // Toggle LED to indicate activity
        static bool ledState = false;
        ledState = !ledState;
        digitalWrite(LED_PIN, ledState);
    }

    LoRaCommand command;
    loraReceiver.receiveLoRaCommand(command);
    if (!command.methodName.empty())
    {
        Serial.printf("Received command: %s\n", command.methodName.c_str());
        // Process the command
        if (command.methodName == "setInterval")
        {
            // Example: setInterval,10
            for (const auto &param : command.parameters)
            {
                if (param.parameterName == "interval")
                {
                    newInterval = atoi(param.parameterValue.c_str());
                    Serial.printf("Setting new interval: %d seconds\n", newInterval);
                }
            }
        }
        else if (command.methodName == "relayOn")
        {
            digitalWrite(RELAY_PIN, HIGH);
            Serial.println("Relay turned ON");
        }
        else if (command.methodName == "relayOff")
        {
            digitalWrite(RELAY_PIN, LOW);
            Serial.println("Relay turned OFF");
        }
    }
#endif

#ifdef RECEIVER
    // Receive binary data via LoRa
    LoRaPacket receivedPacket;
    loraReceiver.receiveSensorData(receivedPacket);
    // Check if a complete packet was received
    if (receivedPacket.packetCount > 0)
    {
        char buffer[256];
        snprintf(buffer, sizeof(buffer),
                 "C:%d, ID:%02X%02X%02X%02X%02X%02X, T:%.2f, H:%.2f, S:%.2f, R:%.2f, WT:%.2f, WF:%.2f, Lon:%.6f, Lat:%.6f\n",
                 receivedPacket.packetCount,
                 receivedPacket.stationMac[0], receivedPacket.stationMac[1], receivedPacket.stationMac[2],
                 receivedPacket.stationMac[3], receivedPacket.stationMac[4], receivedPacket.stationMac[5],
                 receivedPacket.sensorData.temperature,
                 receivedPacket.sensorData.humidity,
                 receivedPacket.sensorData.soilMoisture,
                 receivedPacket.sensorData.rainLevel,
                 receivedPacket.sensorData.waterTemp,
                 receivedPacket.sensorData.totalWaterFlow,
                 receivedPacket.sensorData.longitude,
                 receivedPacket.sensorData.latitude);
        // Send formatted data via custom serial
        customSerial.send(buffer);

        // Flash LED twice to indicate data received
        for (int i = 0; i < 2; i++)
        {
            digitalWrite(LED_PIN, HIGH);
            delay(100);
            digitalWrite(LED_PIN, LOW);
            delay(100);
        }
    }
    std::string command;
    // "C:setInterval,interval=20";
    // loraSender.sendCommand(command);
    // printf("Sent command: %s\n", command.c_str());
    // // Check for LoRa commands

    customSerial.receiveCommand(command);

    if (!command.empty())
    {
        loraSender.sendCommand(command);
    }

    // Toggle LED to indicate activity
    static bool ledState = true;

#endif
}
