#ifndef LORA_SENDER_H
#define LORA_SENDER_H

#include <string>

class LoraSender {
public:
    /**
     * @brief Initialize the LoRa module for sending data.
     */
    void init();

    /**
     * @brief Send sensor data via LoRa.
     *
     * @param temperature The temperature value to send.
     * @param humidity The humidity value to send.
     * @param soilMoisture The soil moisture value to send.
     * @param rainLevel The rain level value to send.
     */
    void sendSensorData(int temperature, int humidity, int soilMoisture, int rainLevel);
private:
    uint8_t station_mac[6];  // MAC address of the device
};

#endif // LORA_SENDER_H