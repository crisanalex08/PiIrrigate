#ifndef RAIN_H
#define RAIN_H

#include <stdint.h>

#ifdef __cplusplus
extern "C" {
#endif

// Define the ADC channel connected to the raindrop sensor
#define RAIN_SENSOR_ADC_CHANNEL ADC1_CHANNEL_0  // GPIO39 (ADC1_CH3)

/**
 * @brief Initialize the raindrop sensor.
 *
 * This function configures the ADC channel and characterizes the ADC.
 */
void rain_sensor_init(void);

/**
 * @brief Read data from the raindrop sensor.
 *
 * This function reads the analog value from the raindrop sensor and converts it to voltage.
 *
 * @return int The voltage value in millivolts (mV).
 */
int rain_sensor_read(void);

#ifdef __cplusplus
}
#endif

#endif // RAIN_H