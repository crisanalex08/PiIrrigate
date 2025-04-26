#include <Arduino.h>
#include "rain.h"
#include "driver/adc.h"
#include "esp_adc_cal.h"

#define DEFAULT_VREF 1100
#define ADC_WIDTH ADC_WIDTH_BIT_12
#define ADC_ATTEN ADC_ATTEN_DB_12

RainSensor::RainSensor() : adc_chars(nullptr) {}

RainSensor::~RainSensor() {
    if (adc_chars) {
        free(adc_chars);
    }
}

void RainSensor::init() {
    // Configure ADC channel
    adc1_config_width(ADC_WIDTH);
    adc1_config_channel_atten(RAIN_SENSOR_ADC_CHANNEL, ADC_ATTEN);

    // Characterize ADC
    adc_chars = (esp_adc_cal_characteristics_t *)calloc(1, sizeof(esp_adc_cal_characteristics_t));
    esp_adc_cal_characterize(ADC_UNIT_1, ADC_ATTEN, ADC_WIDTH, DEFAULT_VREF, adc_chars);
}

int RainSensor::read() {
    // Read raw ADC value
    int raw_value = adc1_get_raw(RAIN_SENSOR_ADC_CHANNEL);

    // Convert raw value to voltage (in mV)
    uint32_t voltage = esp_adc_cal_raw_to_voltage(raw_value, adc_chars);

    // Return the voltage in mV
    return voltage;
}