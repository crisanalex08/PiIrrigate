#include "water_flow.h"
#include <Arduino.h>

static WaterFlowSensor* globalFlowSensor = nullptr;

// ISR function to count pulses
void IRAM_ATTR waterFlowPulseISR() {
    if (globalFlowSensor) {
        globalFlowSensor->_pulseCount++;
    }
}

WaterFlowSensor::WaterFlowSensor(uint8_t pin)
    : _pin(pin), _pulseCount(0), _totalLiters(0.0f), _lastUpdate(0) {}

void WaterFlowSensor::init() {
    pinMode(_pin, INPUT_PULLUP);
    globalFlowSensor = this;
    attachInterrupt(digitalPinToInterrupt(_pin), waterFlowPulseISR, RISING);
    _lastUpdate = millis();
}

void WaterFlowSensor::update() {
    unsigned long now = millis();
    unsigned long elapsed = now - _lastUpdate;
    if (elapsed >= 10000) { // Update every 10 seconds
        // YF-S201: 450 pulses per liter
        uint32_t pulses = _pulseCount;
        _pulseCount = 0;
        _totalLiters += (pulses / 450.0f);
        _lastUpdate = now;
    }
}

float WaterFlowSensor::getTotalLiters() const {
    return _totalLiters;
}