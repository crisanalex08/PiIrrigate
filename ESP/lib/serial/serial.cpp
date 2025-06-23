#include "serial.h"
#include "driver/uart.h"
#include "esp_system.h"
#include "esp_log.h"
#include "esp_mac.h"
#include <cstring>
#include <cstdio>
#include <WString.h>

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

    uart_driver_install(UART_NUM, BUF_SIZE, 0, 0, NULL, 0);

    ESP_LOGI("UART", "UART initialized on USB (UART0)");
}

void MySerial::send(const std::string &data) {
    // Send a string of data via UART
    uart_write_bytes(UART_NUM, data.c_str(), data.length());
}

void MySerial::sendSensorData(int packetCount, const SensorData& sensorData) {
    char buffer[256];

    snprintf(buffer, sizeof(buffer),
        "CNT:%d, ID:%02X%02X%02X%02X%02X%02X, T:%.2f, H:%.2f, S:%.2f, R:%.2f, WT:%.2f, WF:%.2f, Lon:%.6f, Lat:%.6f, CT:%lu, W:% \n",
        packetCount,
        station_mac[0], station_mac[1], station_mac[2], station_mac[3], station_mac[4], station_mac[5],
        sensorData.temperature,
        sensorData.humidity,
        sensorData.soilMoisture,
        sensorData.rainLevel,
        sensorData.waterTemp,
        sensorData.totalWaterFlow,
        sensorData.longitude,
        sensorData.latitude,
        sensorData.readingTime,
        sensorData.watering
    );

    send(buffer);
}

void MySerial::receiveCommand(std::string &command) {
    // Receive a command via UART
    char buffer[256];
    int len = uart_read_bytes(UART_NUM, (uint8_t *)buffer, sizeof(buffer) - 1, 100 / portTICK_RATE_MS);
    if (len > 0) {
        buffer[len] = '\0'; // Null-terminate the string
        command = std::string(buffer);
        ESP_LOGI("UART", "Received command: %s", command.c_str());
    } else {
        command.clear(); // Clear command if no data received
    }
}