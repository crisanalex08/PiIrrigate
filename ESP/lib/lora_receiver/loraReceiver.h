#ifndef LORA_RECEIVER_H
#define LORA_RECEIVER_H

#include <string>
#include <sensor_data.h>


class LoraReceiver {
public:
    /**
     * @brief Initialize the LoRa module for receiving data.
     */
    void init();

    /**
     * @brief Receive sensor data via LoRa.
     *
     * @return std::string The received data as a string.
     */
    bool receiveSensorData(LoRaPacket& outPacket);
};

#endif // LORA_RECEIVER_H