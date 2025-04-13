#ifndef SOIL_H
#define SOIL_H

#include <stdint.h>

#ifdef __cplusplus
extern "C" {
#endif

// Define the ADC channel connected to the soil moisture sensor
#define SOIL_SENSOR_ADC_CHANNEL ADC1_CHANNEL_3  // GPIO36 (ADC1_CH0)

/**
 * @brief Initialize the soil moisture sensor.
 *
 * This function configures the ADC channel and characterizes the ADC.
 */
void soil_sensor_init(void);

/**
 * @brief Read data from the soil moisture sensor.
 *
 * This function reads the analog value from the soil moisture sensor and converts it to voltage.
 *
 * @return int The voltage value in millivolts (mV).
 */
int soil_sensor_read(void);

#ifdef __cplusplus
}
#endif

#endif // SOIL_H