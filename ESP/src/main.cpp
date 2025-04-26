#include <Arduino.h>
#include "driver/gpio.h"
#include <dht.h>
#include <rain.h>
#include <soil.h>
#include <serial.h>
#include <LoraSender.h>
#include <LoraReceiver.h>

// Define GPIO pin for an LED (example: GPIO2)
#define LED_PIN GPIO_NUM_2

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
    customSerial.sendSensorData(temperature, humidity, soilMoisture, rainLevel);

    // Send sensor data via LoRa
    loraSender.sendSensorData(temperature, humidity, soilMoisture, rainLevel);

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
    }

    // Toggle LED to indicate activity
    static bool ledState = false;
    ledState = !ledState;
    digitalWrite(LED_PIN, ledState);

    // Wait 2 seconds before checking for the next packet
    delay(2000);
#endif
}