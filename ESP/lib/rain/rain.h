#ifndef RAIN_H
#define RAIN_H

#include <stdint.h>
#include "driver/adc.h"
#include "esp_adc_cal.h"

// Define the ADC channel connected to the raindrop sensor
#define RAIN_SENSOR_ADC_CHANNEL ADC1_CHANNEL_0  // GPIO39 (ADC1_CH0)

class RainSensor {
public:
    /**
     * @brief Constructor for the RainSensor class.
     */
    RainSensor();

    /**
     * @brief Destructor for the RainSensor class.
     */
    ~RainSensor();

    /**
     * @brief Initialize the raindrop sensor.
     *
     * This function configures the ADC channel and characterizes the ADC.
     */
    void init();

    /**
     * @brief Read data from the raindrop sensor.
     *
     * This function reads the analog value from the raindrop sensor and converts it to voltage.
     *
     * @return int The voltage value in millivolts (mV).
     */
    int read();

private:
    esp_adc_cal_characteristics_t *adc_chars;  // ADC calibration characteristics
};

#endif // RAIN_H