#include "serial.h"
#include "driver/uart.h"
#include "esp_system.h"
#include "esp_log.h"
#include "esp_mac.h"
#include <cstring>
#include <cstdio>

#define UART_NUM UART_NUM_0  // Use UART0 for USB communication
#define BAUD_RATE 115200     // Baud rate
#define BUF_SIZE 1024        // UART buffer size

void MySerial::init() {
    // Configure UART parameters
    uart_config_t uart_config = {
        .baud_rate = BAUD_RATE,
        .data_bits = UART_DATA_8_BITS,
        .parity = UART_PARITY_DISABLE,
        .stop_bits = UART_STOP_BITS_1,
        .flow_ctrl = UART_HW_FLOWCTRL_DISABLE
    };
    uart_param_config(UART_NUM, &uart_config);

    // Read the MAC address from the Wi-Fi station
    esp_err_t ret = esp_read_mac(station_mac, ESP_MAC_WIFI_STA);
    if (ret != ESP_OK) {
        ESP_LOGE("UART", "Failed to read MAC address");
    }

    // Install UART driver
    uart_driver_install(UART_NUM, BUF_SIZE, 0, 0, NULL, 0);

    ESP_LOGI("UART", "UART initialized on USB (UART0)");
}

void MySerial::send(const std::string &data) {
    // Send a string of data via UART
    uart_write_bytes(UART_NUM, data.c_str(), data.length());
}

void MySerial::sendSensorData(int temperature, int humidity, int soil_moisture, int rain_level) {
    char buffer[128];

    // Format the sensor data into a string, including the unique identifier
    snprintf(buffer, sizeof(buffer),
             "ID:%02X%02X%02X%02X%02X%02X, T:%d, H:%d, S:%d, R:%d\n",
             station_mac[0], station_mac[1], station_mac[2], station_mac[3], station_mac[4], station_mac[5],
             temperature, humidity, soil_moisture, rain_level);

    // Send the formatted string via UART
    send(buffer);
}