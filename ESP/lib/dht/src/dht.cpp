#include <Arduino.h>
#include "dht.h"
#include "driver/gpio.h"

#define DHT_GPIO GPIO_NUM_13  // GPIO pin connected to DHT11

void DHT::init() {
    pinMode(DHT_GPIO, OUTPUT);
    digitalWrite(DHT_GPIO, HIGH);  // Set default state to HIGH
}

int DHT::read(int &humidity, int &temperature) {
    uint8_t data[5] = {0};
    int bitIndex = 0;

    // Send start signal
    pinMode(DHT_GPIO, OUTPUT);
    digitalWrite(DHT_GPIO, LOW);
    delay(20);  // Pull low for at least 18ms
    digitalWrite(DHT_GPIO, HIGH);
    delayMicroseconds(30);  // Pull high for 20-40us

    // Set GPIO to input mode
    pinMode(DHT_GPIO, INPUT);

    // Wait for DHT response
    if (waitForSignal(LOW, 1000) < 0) return -1;  // Wait for low signal
    if (waitForSignal(HIGH, 1000) < 0) return -1; // Wait for high signal
    if (waitForSignal(LOW, 1000) < 0) return -1;  // Wait for low signal

    // Read 40 bits (5 bytes) of data
    for (int i = 0; i < 40; i++) {
        if (waitForSignal(HIGH, 1000) < 0) return -1;  // Wait for high signal
        delayMicroseconds(30);  // Wait for 30us
        if (digitalRead(DHT_GPIO) == HIGH) {
            data[bitIndex / 8] |= (1 << (7 - (bitIndex % 8)));  // Set bit
        }
        if (waitForSignal(LOW, 1000) < 0) return -1;  // Wait for low signal
        bitIndex++;
    }

    // Verify checksum
    if (data[4] != (data[0] + data[1] + data[2] + data[3])) return -1;

    // Extract humidity and temperature
    humidity = data[0];
    temperature = data[2];

    return 0;
}

int DHT::waitForSignal(int signal, int timeout) {
    int counter = 0;
    while (digitalRead(DHT_GPIO) != signal) {
        if (++counter > timeout) return -1;  // Timeout
        delayMicroseconds(1);
    }
    return 0;
}