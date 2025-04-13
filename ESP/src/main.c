#include <stdio.h>
#include "freertos/FreeRTOS.h"
#include "freertos/task.h"
#include "driver/gpio.h"
#include "dht.h"
#include "soil.h"
#include "rain.h"
#include "serial.h"

void app_main() {
    int humidity = 0;
    int temperature = 0;
    int soil_voltage = 0;
    int soil_moisture = 0;
    int rain_voltage = 0;
    int rain_level = 0;

    // Initialize sensors
    dht_init();
    soil_sensor_init();
    rain_sensor_init();
    serial_init();  // Initialize serial communication

    while (1) {
        // Read temperature and humidity from DHT11 sensor
        if (!dht_read(&humidity, &temperature) == 0) {  // Check if read was unsuccessful
            temperature  = 0;  // Set to 0 if read fails
            humidity = 0;     // Set to 0 if read fails
        }

        // Read soil moisture sensor voltage
        soil_voltage = soil_sensor_read();
        soil_moisture = 100 - ((soil_voltage * 100) / 3300);  // Map voltage to percentage

        // Read raindrop sensor voltage
        rain_voltage = rain_sensor_read();
        rain_level = 100 - ((rain_voltage * 100) / 3300);  // Map voltage to percentage
   
        serial_send_sensor_data(temperature, humidity, soil_moisture, rain_level);  // Send data via UART
        // Wait for 2 seconds before the next reading
        vTaskDelay(pdMS_TO_TICKS(2000));
    }
}