#include "LoraSender.h"
#include <LoRa.h>
#include <Arduino.h>

#define LORA_SS 18    // GPIO 18 for SS
#define LORA_RST 23   // GPIO 23 for Reset
#define LORA_DIO0 26  // GPIO 26 for DIO0

void LoraSender::init() {
    // Initialize the LoRa module
    if (!LoRa.begin(433E6)) {  // Set frequency to 433 MHz
        Serial.println("LoRa initialization failed!");
        while (1);  // Halt execution if LoRa initialization fails
    }
    Serial.println("LoRa Sender initialized");
    // Read the MAC address from the Wi-Fi station
    esp_err_t ret = esp_read_mac(station_mac, ESP_MAC_WIFI_STA);
}

void LoraSender::sendSensorData(int temperature, int humidity, int soilMoisture, int rainLevel) {
    char buffer[128];

    snprintf(buffer, sizeof(buffer),
             "ID:%02X%02X%02X%02X%02X%02X, T:%d, H:%d, S:%d, R:%d\n",
             station_mac[0], station_mac[1], station_mac[2], station_mac[3], station_mac[4], station_mac[5],
             temperature, humidity, soilMoisture, rainLevel);
    // Begin LoRa packet
    LoRa.beginPacket();
    LoRa.print(buffer);  // Send the formatted string
    LoRa.endPacket();

    Serial.println("LoRa data sent: " + String(buffer));
}