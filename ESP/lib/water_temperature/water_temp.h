#ifndef WATER_TEMP_H
#define WATER_TEMP_H

#include <stdint.h>

/**
 * @brief Class to interface with a DS18B20 temperature sensor.
 *        Assumes a single DS18B20 is connected and bit-bangs the protocol.
 */
class WaterTempSensor {
public:
    /**
     * @brief Constructor.
     * @param pin GPIO pin connected to the DS18B20 data line.
     */
    WaterTempSensor(uint8_t pin);

    /**
     * @brief Initialize the DS18B20 sensor pin.
     */
    void init();

    /**
     * @brief Read the water temperature in Celsius.
     * @return float Temperature in Celsius, or NAN on error.
     */
    float readTemperature();

private:
    uint8_t _pin;

    // Low-level protocol helpers
    void writeBit(bool bit);
    bool readBit();
    void writeByte(uint8_t byte);
    uint8_t readByte();
    bool resetPulse();
};

#endif // WATER_TEMP_H