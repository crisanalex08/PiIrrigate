#ifndef TIME_UTILS_H
#define TIME_UTILS_H

#include <time.h>

/**
 * @brief Initialize WiFi and synchronize time using NTP.
 *        Call this once in setup().
 */
void initMyTime();

/**
 * @brief Get the current epoch time (seconds since Jan 1, 1970).
 * @return time_t Current epoch time.
 */
time_t getEpochTime();

#endif // TIME_UTILS_H