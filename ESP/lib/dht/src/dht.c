#include <stdio.h>
#include "freertos/FreeRTOS.h"
#include "freertos/task.h"
#include "driver/gpio.h"

#define DHT_GPIO GPIO_NUM_13  // GPIO pin connected to DHT11

// Function to initialize the DHT11 sensor
void dht_init() {
    esp_rom_gpio_pad_select_gpio(DHT_GPIO);
    gpio_set_direction(DHT_GPIO, GPIO_MODE_OUTPUT);
}

// Function to read data from the DHT11 sensor
int dht_read(int *humidity, int *temperature) {
    uint8_t data[5] = {0};
    int bit_index = 0;

    // Send start signal
    gpio_set_direction(DHT_GPIO, GPIO_MODE_OUTPUT);
    gpio_set_level(DHT_GPIO, 0);
    vTaskDelay(pdMS_TO_TICKS(20));  // Pull low for at least 18ms
    gpio_set_level(DHT_GPIO, 1);
    esp_rom_delay_us(30);  // Pull high for 20-40us

    // Set GPIO to input mode
    gpio_set_direction(DHT_GPIO, GPIO_MODE_INPUT);

    // Wait for DHT response
    int timeout = 0;
    while (gpio_get_level(DHT_GPIO) == 1) {
        if (++timeout > 1000) return -1;  // Timeout waiting for response
        esp_rom_delay_us(1);
    }
    timeout = 0;
    while (gpio_get_level(DHT_GPIO) == 0) {
        if (++timeout > 1000) return -1;  // Timeout waiting for low signal
        esp_rom_delay_us(1);
    }
    timeout = 0;
    while (gpio_get_level(DHT_GPIO) == 1) {
        if (++timeout > 1000) return -1;  // Timeout waiting for high signal
        esp_rom_delay_us(1);
    }

    // Read 40 bits (5 bytes) of data
    for (int i = 0; i < 40; i++) {
        timeout = 0;
        while (gpio_get_level(DHT_GPIO) == 0) {  // Wait for low signal
            if (++timeout > 1000) return -1;  // Timeout
            esp_rom_delay_us(1);
        }
        esp_rom_delay_us(30);  // Wait for 30us
        if (gpio_get_level(DHT_GPIO) == 1) {
            data[bit_index / 8] |= (1 << (7 - (bit_index % 8)));  // Set bit
        }
        timeout = 0;
        while (gpio_get_level(DHT_GPIO) == 1) {  // Wait for high signal to end
            if (++timeout > 1000) return -1;  // Timeout
            esp_rom_delay_us(1);
        }
        bit_index++;
    }

    // Verify checksum
    if (data[4] != (data[0] + data[1] + data[2] + data[3])) return -1;

    // Extract humidity and temperature
    *humidity = data[0];
    *temperature = data[2];

    return 0;
}