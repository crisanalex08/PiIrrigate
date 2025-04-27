#include "LoraSender.h"
#include <LoRa.h>
#include <Arduino.h>

#define LORA_SS 18   // GPIO 18 for SS
#define LORA_RST 23  // GPIO 23 for Reset
#define LORA_DIO0 26 // GPIO 26 for DIO0
#define LED_PIN GPIO_NUM_4

void LoraSender::init()
{
    // Initialize the LoRa module
    if (!LoRa.begin(433E6))
    { // Set frequency to 433 MHz
        Serial.println("LoRa initialization failed!");
        while (1)
            ; // Halt execution if LoRa initialization fails
    }
    Serial.println("LoRa Sender initialized");
    // Read the MAC address from the Wi-Fi station
    esp_err_t ret = esp_read_mac(station_mac, ESP_MAC_WIFI_STA);
}

void LoraSender::sendSensorData(int temperature, int humidity, int soilMoisture, int rainLevel)
{
    char buffer[128];

    snprintf(buffer, sizeof(buffer),
             "ID:%02X%02X%02X%02X%02X%02X, T:%d, H:%d, S:%d, R:%d\n",
             station_mac[0], station_mac[1], station_mac[2], station_mac[3], station_mac[4], station_mac[5],
             temperature, humidity, soilMoisture, rainLevel);
    // Begin LoRa packet
    LoRa.beginPacket();
    LoRa.print(buffer); // Send the formatted string
    LoRa.endPacket();

    Serial.println("LoRa data sent: " + String(buffer));
}

void LoraSender::sendSensorDataWithAck(int packetCount, int temperature, int humidity, int soilMoisture, int rainLevel)
{
    char buffer[128];

    snprintf(buffer, sizeof(buffer),
             "C:%d, ID:%02X%02X%02X%02X%02X%02X, T:%d, H:%d, S:%d, R:%d\n",
             packetCount, station_mac[0], station_mac[1], station_mac[2], station_mac[3], station_mac[4], station_mac[5],
             temperature, humidity, soilMoisture, rainLevel);

    for (int attempt = 0; attempt < 3; attempt++)
    { // Retry up to 3 times
        // Begin LoRa packet
        LoRa.beginPacket();
        LoRa.print(buffer);   // Send the formatted string
        LoRa.endPacket(true); // Wait for acknowledgment

        // Wait for acknowledgment
        unsigned long startTime = millis();
        while (millis() - startTime < 2000)
        { // 2-second timeout
            int packetSize = LoRa.parsePacket();
            if (packetSize > 0)
            {
                String ack = "";
                while (LoRa.available())
                {
                    ack += (char)LoRa.read();
                }
                if (ack == "ACK")
                {
                    Serial.println("Acknowledgment received!");
                    return; // Exit after successful acknowledgment
                }
            }
        }

        digitalWrite(LED_PIN, LOW);  // Turn off LED to indicate failure
        delay(100);                  // Wait for 100ms
        digitalWrite(LED_PIN, HIGH); // Turn on LED
        Serial.println("No acknowledgment, retrying...");
    }
    Serial.println("Failed to send packet after 3 attempts");
}