#include "rain.h"
#include "driver/adc.h"
#include "esp_adc_cal.h"

#define DEFAULT_VREF 1100
#define ADC_WIDTH ADC_WIDTH_BIT_12
#define ADC_ATTEN ADC_ATTEN_DB_12

static esp_adc_cal_characteristics_t *adc_chars;

void rain_sensor_init() {
    adc1_config_width(ADC_WIDTH);
    adc1_config_channel_atten(RAIN_SENSOR_ADC_CHANNEL, ADC_ATTEN);

    adc_chars = calloc(1, sizeof(esp_adc_cal_characteristics_t));
    esp_adc_cal_characterize(ADC_UNIT_1, ADC_ATTEN, ADC_WIDTH, DEFAULT_VREF, adc_chars);
}

int rain_sensor_read() {
    int raw_value = adc1_get_raw(RAIN_SENSOR_ADC_CHANNEL);
    uint32_t voltage = esp_adc_cal_raw_to_voltage(raw_value, adc_chars);
    return voltage;
}