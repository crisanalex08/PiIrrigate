#ifndef SERIAL_H
#define SERIAL_H

#include <stdint.h>
#include <string>

class MySerial {
public:
    /**
     * @brief Initialize the UART for serial communication.
     *
     * This function configures the UART with the specified baud rate and pins.
     */
    void init();

    /**
     * @brief Send a string of data via UART.
     *
     * @param data The string to send.
     */
    void send(const std::string &data);

    /**
     * @brief Send formatted sensor data via UART.
     *
     * @param temperature The temperature value to send.
     * @param humidity The humidity value to send.
     * @param soil_moisture The soil moisture percentage to send.
     * @param rain_level The rain level percentage to send.
     */
    void sendSensorData(int temperature, int humidity, int soil_moisture, int rain_level);

private:
    uint8_t station_mac[6];  // MAC address of the device
};

#endif // SERIAL_H