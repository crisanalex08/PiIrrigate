//YF-S201

#ifndef WATER_FLOW_H
#define WATER_FLOW_H

#include <stdint.h>

/**
 * @brief Class to interface with a YF-S201 water flow sensor.
 *        Counts pulses on the signal pin to calculate total liters.
 */
class WaterFlowSensor {
public:
    volatile uint32_t _pulseCount;
    /**
     * @brief Constructor.
     * @param pin GPIO pin connected to the YF-S201 signal output.
     */
    WaterFlowSensor(uint8_t pin);

    /**
     * @brief Initialize the sensor pin and interrupt.
     */
    void init();

    /**
     * @brief Call this in your loop to update the total liters calculation.
     */
    void update();

    /**
     * @brief Get the total volume passed through the sensor in liters.
     * @return float Total volume (L).
     */
    float getTotalLiters() const;

private:
    uint8_t _pin;
    float _totalLiters;
    unsigned long _lastUpdate;
};

void waterFlowPulseISR();

#endif // WATER_FLOW_H

