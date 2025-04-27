#include <Arduino.h>
#include "driver/gpio.h"
#include <dht.h>
#include <rain.h>
#include <soil.h>
#include <serial.h>
#include <LoraSender.h>
#include <LoraReceiver.h>

// Define GPIO pin for an LED (example: GPIO2)
#define LED_PIN GPIO_NUM_4
#define MESSAGE_TIMEOUT 20000  // 2 seconds timeout for acknowledgment
int packetCount = 0; // Packet count for debugging

DHT dht;
RainSensor rainSensor;
SoilSensor soilSensor;
MySerial customSerial;

#ifdef SENDER
LoraSender loraSender;
#endif

#ifdef RECEIVER
LoraReceiver loraReceiver;
#endif

void setup()
{
    // Initialize the serial communication
    Serial.begin(115200);
    customSerial.init();

#ifdef SENDER
    // Initialize sensors
    dht.init();
    rainSensor.init();
    soilSensor.init();

    // Initialize LoRa sender
    loraSender.init();
#endif

#ifdef RECEIVER
    // Initialize LoRa receiver
    loraReceiver.init();
#endif

    // Configure the LED pin as output
    pinMode(LED_PIN, OUTPUT);

    Serial.println("System initialized");
}

void loop()
{
#ifdef SENDER
    int humidity = 0, temperature = 0;
    int soilMoisture = 0, rainLevel = 0;

    // Read data from DHT sensor
    if (dht.read(humidity, temperature) != 0)
    {
        Serial.println("Failed to read from DHT sensor");
    }

    // Read data from RainSensor
    rainLevel = rainSensor.read();

    // Read data from SoilSensor
    soilMoisture = soilSensor.read();

    // Send sensor data via custom serial
    customSerial.sendSensorData(packetCount, temperature, humidity, soilMoisture, rainLevel);

    // Send sensor data via LoRa
    loraSender.sendSensorDataWithAck(packetCount, temperature, humidity, soilMoisture, rainLevel);
    packetCount++; // Increment packet count for the next message
    // Toggle LED to indicate activity
    static bool ledState = false;
    ledState = !ledState;
    digitalWrite(LED_PIN, ledState);

    // Wait 10 seconds before the next reading
    delay(10000);
#endif

#ifdef RECEIVER
    // Receive data via LoRa
    std::string receivedData = loraReceiver.receiveSensorData();
    if (!receivedData.empty())
    {
        // Send received data via custom serial
        customSerial.send(receivedData);
        // Flash LED twice to indicate data received
        for (int i = 0; i < 2; i++)
        {
            digitalWrite(LED_PIN, HIGH);
            delay(100);
            digitalWrite(LED_PIN, LOW);
            delay(100);
        }
    }
    // Toggle LED to indicate activity
    static bool ledState = true;
#endif
}