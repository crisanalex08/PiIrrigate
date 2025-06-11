#ifndef GPS_H
#define GPS_H

#include <stdint.h>

/**
 * @brief Class to interface with a NEO-6M GPS module.
 *        Assumes the GPS is connected via a UART port.
 */
class GpsModule {
public:
    /**
     * @brief Constructor.
     * @param uart_num UART port number (e.g., UART_NUM_1)
     * @param tx_pin GPIO pin for TX
     * @param rx_pin GPIO pin for RX
     * @param baud_rate Baud rate for GPS communication (default 9600)
     */
    GpsModule(int uart_num, int tx_pin, int rx_pin, uint32_t baud_rate = 9600);

    /**
     * @brief Initialize the GPS UART.
     */
    void init();

    /**
     * @brief Update GPS data by reading from UART.
     *        Call this frequently in your loop.
     */
    void update();

    /**
     * @brief Get the latest latitude value.
     * @return double Latitude in decimal degrees.
     */
    double getLatitude() const;

    /**
     * @brief Get the latest longitude value.
     * @return double Longitude in decimal degrees.
     */
    double getLongitude() const;

    /**
     * @brief Check if the GPS has a valid fix.
     * @return true if fix is valid, false otherwise.
     */
    bool hasFix() const;

private:
    int _uart_num;
    int _tx_pin;
    int _rx_pin;
    uint32_t _baud_rate;
    double _latitude;
    double _longitude;
    bool _hasFix;

    void parseNmeaSentence(const char* sentence);
};

#endif // GPS_H