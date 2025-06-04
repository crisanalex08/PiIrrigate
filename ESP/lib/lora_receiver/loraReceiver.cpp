#include "LoraReceiver.h"
#include <LoRa.h>
#include <Arduino.h>
#include <sensor_data.h>

void LoraReceiver::init() {
    // Initialize the LoRa module
    if (!LoRa.begin(433E6)) {  // Set frequency to 433 MHz
        Serial.println("LoRa initialization failed!");
        while (1);  // Halt execution if LoRa initialization fails
    }
    Serial.println("LoRa Receiver initialized");
}

bool LoraReceiver::receiveSensorData(LoRaPacket& outPacket) {
    int packetSize = LoRa.parsePacket();  // Check if a packet is available
    if (packetSize == sizeof(LoRaPacket)) {
        // Read the binary packet directly into the struct
        LoRa.readBytes(reinterpret_cast<uint8_t*>(&outPacket), sizeof(LoRaPacket));

        // Send acknowledgment
        LoRa.beginPacket();
        LoRa.print("ACK");
        LoRa.endPacket();

        return true;
    }
    return false;
}