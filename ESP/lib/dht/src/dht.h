#ifndef DHT_H
#define DHT_H

#include <stdint.h>

class DHT {
public:
    /**
     * @brief Initialize the DHT sensor.
     *
     * This function configures the GPIO pin connected to the DHT sensor.
     */
    void init();

    /**
     * @brief Read data from the DHT sensor.
     *
     * This function reads temperature and humidity data from the DHT sensor.
     *
     * @param[out] humidity Reference to store the humidity value (0-100%).
     * @param[out] temperature Reference to store the temperature value (in °C).
     * @return int Returns 0 on success, or -1 on failure.
     */
    int read(int &humidity, int &temperature);

private:
    /**
     * @brief Wait for a specific signal (HIGH or LOW) with a timeout.
     *
     * @param signal The signal to wait for (HIGH or LOW).
     * @param timeout Timeout in microseconds.
     * @return int Returns 0 on success, or -1 on timeout.
     */
    int waitForSignal(int signal, int timeout);
};

#endif // DHT_H