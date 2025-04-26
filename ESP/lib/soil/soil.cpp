#include "soil.h"
#include <Arduino.h>

#define DEFAULT_VREF 1100  // Default reference voltage in mV (adjust if needed)
#define ADC_WIDTH ADC_WIDTH_BIT_12  // ADC resolution (12 bits)
#define ADC_ATTEN ADC_ATTEN_DB_12   // ADC attenuation (0-3.3V range)

SoilSensor::SoilSensor() : adc_chars(nullptr) {}

SoilSensor::~SoilSensor() {
    if (adc_chars) {
        free(adc_chars);
    }
}

void SoilSensor::init() {
    // Configure ADC channel
    adc1_config_width(ADC_WIDTH);
    adc1_config_channel_atten(SOIL_SENSOR_ADC_CHANNEL, ADC_ATTEN);

    // Characterize ADC
    adc_chars = (esp_adc_cal_characteristics_t *)calloc(1, sizeof(esp_adc_cal_characteristics_t));
    esp_adc_cal_characterize(ADC_UNIT_1, ADC_ATTEN, ADC_WIDTH, DEFAULT_VREF, adc_chars);
}

int SoilSensor::read() {
    // Read raw ADC value
    int raw_value = adc1_get_raw(SOIL_SENSOR_ADC_CHANNEL);

    // Convert raw value to voltage (in mV)
    uint32_t voltage = esp_adc_cal_raw_to_voltage(raw_value, adc_chars);

    // Return the voltage in mV
    return voltage;
}