#include "serial.h"
#include "driver/uart.h"
#include "esp_system.h"
#include "esp_log.h"
#include "esp_mac.h"
#include <string.h>

#define TAG "BASE_MAC"
#define UART_NUM UART_NUM_0  // Use UART0 for USB communication
#define BAUD_RATE 115200     // Baud rate
#define BUF_SIZE 1024        // UART buffer size
uint8_t station_mac[6];

void serial_init() {
    // Configure UART parameters
    uart_config_t uart_config = {
        .baud_rate = BAUD_RATE,
        .data_bits = UART_DATA_8_BITS,
        .parity = UART_PARITY_DISABLE,
        .stop_bits = UART_STOP_BITS_1,
        .flow_ctrl = UART_HW_FLOWCTRL_DISABLE
    };
    uart_param_config(UART_NUM, &uart_config);
    esp_err_t ret = ESP_OK;
    ret = esp_read_mac(station_mac, ESP_MAC_WIFI_STA);  // Read the MAC address from wifi station

    // Install UART driver
    uart_driver_install(UART_NUM, BUF_SIZE, 0, 0, NULL, 0);

    ESP_LOGI("UART", "UART initialized on USB (UART0)");
}

void serial_send(const char *data) {
    // Send a string of data via UART
    uart_write_bytes(UART_NUM, data, strlen(data));
}

void serial_send_sensor_data(int temperature, int humidity, int soil_moisture, int rain_level) {
    char buffer[128];

        // Format the sensor data into a string, including the unique identifier
        snprintf(buffer, sizeof(buffer),
        "ID:%02X%02X%02X%02X%02X%02X, T:%d, H:%d, S:%d, R:%d\n",
        station_mac[0], station_mac[1], station_mac[2], station_mac[3], station_mac[4], station_mac[5],
        temperature, humidity, soil_moisture, rain_level);

    // Send the formatted string via UART
    serial_send(buffer);
}