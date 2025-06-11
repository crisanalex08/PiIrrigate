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
     * @param outPacket The LoRaPacket structure to fill with received data.
     * @return true if a complete packet was received, false otherwise.
     */
    bool receiveSensorData(LoRaPacket& outPacket);

    /**
     * @brief Receive commands via LoRa
     *
     * @param command The LoRaCommand structure to fill with received command data.
     */
    void receiveLoRaCommand(LoRaCommand& command);
};

#endif // LORA_RECEIVER_H