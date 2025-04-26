#include "LoraReceiver.h"
#include <LoRa.h>
#include <Arduino.h>

void LoraReceiver::init() {
    // Initialize the LoRa module
    if (!LoRa.begin(433E6)) {  // Set frequency to 433 MHz
        Serial.println("LoRa initialization failed!");
        while (1);  // Halt execution if LoRa initialization fails
    }
    Serial.println("LoRa Receiver initialized");
}

std::string LoraReceiver::receiveSensorData() {
    int packetSize = LoRa.parsePacket();  // Check if a packet is available
    if (packetSize > 0) {
        char buffer[packetSize + 1];
        int index = 0;

        // Read the packet data
        while (LoRa.available() && index < packetSize) {
            buffer[index++] = (char)LoRa.read();
        }
        buffer[index] = '\0';  // Null-terminate the string
        return std::string(buffer);
    }
    return "";
}