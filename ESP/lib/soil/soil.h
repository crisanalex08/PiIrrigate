#ifndef SOIL_H
#define SOIL_H

#include <stdint.h>
#include "driver/adc.h"
#include "esp_adc_cal.h"

// Define the ADC channel connected to the soil moisture sensor
#define SOIL_SENSOR_ADC_CHANNEL ADC1_CHANNEL_1  // Example: GPIO36 (ADC1_CH1)

class SoilSensor {
public:
    /**
     * @brief Constructor for the SoilSensor class.
     */
    SoilSensor();

    /**
     * @brief Destructor for the SoilSensor class.
     */
    ~SoilSensor();

    /**
     * @brief Initialize the soil moisture sensor.
     *
     * This function configures the ADC channel and characterizes the ADC.
     */
    void init();

    /**
     * @brief Read data from the soil moisture sensor.
     *
     * This function reads the analog value from the soil moisture sensor and converts it to voltage.
     *
     * @return int The voltage value in millivolts (mV).
     */
    int read();

private:
    esp_adc_cal_characteristics_t *adc_chars;  // ADC calibration characteristics
};

#endif // SOIL_H