#include "water_temp.h"
#include <Arduino.h>

WaterTempSensor::WaterTempSensor(uint8_t pin) : _pin(pin) {}

void WaterTempSensor::init() {
    pinMode(_pin, OUTPUT);
    digitalWrite(_pin, HIGH);
}

bool WaterTempSensor::resetPulse() {
    pinMode(_pin, OUTPUT);
    digitalWrite(_pin, LOW);
    delayMicroseconds(480);
    pinMode(_pin, INPUT_PULLUP);
    delayMicroseconds(70);
    bool presence = !digitalRead(_pin);
    delayMicroseconds(410);
    return presence;
}

void WaterTempSensor::writeBit(bool bit) {
    pinMode(_pin, OUTPUT);
    digitalWrite(_pin, LOW);
    if (bit) {
        delayMicroseconds(6);
        pinMode(_pin, INPUT_PULLUP);
        delayMicroseconds(64);
    } else {
        delayMicroseconds(60);
        pinMode(_pin, INPUT_PULLUP);
        delayMicroseconds(10);
    }
}

bool WaterTempSensor::readBit() {
    pinMode(_pin, OUTPUT);
    digitalWrite(_pin, LOW);
    delayMicroseconds(6);
    pinMode(_pin, INPUT_PULLUP);
    delayMicroseconds(9);
    bool bit = digitalRead(_pin);
    delayMicroseconds(55);
    return bit;
}

void WaterTempSensor::writeByte(uint8_t byte) {
    for (int i = 0; i < 8; i++) {
        writeBit(byte & 0x01);
        byte >>= 1;
    }
}

uint8_t WaterTempSensor::readByte() {
    uint8_t byte = 0;
    for (int i = 0; i < 8; i++) {
        if (readBit()) {
            byte |= (1 << i);
        }
    }
    return byte;
}

float WaterTempSensor::readTemperature() {
    if (!resetPulse()) {
        return NAN;
    }
    writeByte(0xCC); // Skip ROM
    writeByte(0x44); // Convert T

    // Wait for conversion (typical max 750ms for 12-bit)
    delay(750);

    if (!resetPulse()) {
        return NAN;
    }
    writeByte(0xCC); // Skip ROM
    writeByte(0xBE); // Read Scratchpad

    uint8_t lsb = readByte();
    uint8_t msb = readByte();

    int16_t raw = (msb << 8) | lsb;
    return (float)raw / 16.0;
}