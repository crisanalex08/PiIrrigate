#ifndef SERIAL_H
#define SERIAL_H

#include <stdint.h>

#ifdef __cplusplus
extern "C" {
#endif

/**
 * @brief Initialize the UART for serial communication.
 *
 * This function configures the UART with the specified baud rate and pins.
 */
void serial_init(void);

/**
 * @brief Send a string of data via UART.
 *
 * @param data The string to send.
 */
void serial_send(const char *data);

/**
 * @brief Send formatted sensor data via UART.
 *
 * @param temperature The temperature value to send.
 * @param humidity The humidity value to send.
 * @param soil_moisture The soil moisture percentage to send.
 * @param rain_level The rain level percentage to send.
 */
void serial_send_sensor_data(int temperature, int humidity, int soil_moisture, int rain_level);

#ifdef __cplusplus
}
#endif

#endif // SERIAL_H