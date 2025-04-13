#ifndef DHT_H
#define DHT_H

#include <stdint.h>

// Define the GPIO pin connected to the DHT sensor
#define DHT_GPIO GPIO_NUM_13

#ifdef __cplusplus
extern "C" {
#endif

/**
 * @brief Initialize the DHT sensor.
 *
 * This function configures the GPIO pin connected to the DHT sensor.
 */
void dht_init(void);

/**
 * @brief Read data from the DHT sensor.
 *
 * This function reads temperature and humidity data from the DHT sensor.
 *
 * @param[out] humidity Pointer to store the humidity value (0-100%).
 * @param[out] temperature Pointer to store the temperature value (in °C).
 * @return int Returns 0 on success, or -1 on failure.
 */
int dht_read(int *humidity, int *temperature);

#ifdef __cplusplus
}
#endif

#endif // DHT_H