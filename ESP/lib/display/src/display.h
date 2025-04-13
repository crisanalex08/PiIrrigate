#ifndef DISPLAY_H
#define DISPLAY_H

#include <u8g2.h>

#ifdef __cplusplus
extern "C" {
#endif

/**
 * @brief Initialize the display.
 *
 * This function initializes the display using the U8g2 library.
 */
void display_init(void);

/**
 * @brief Display temperature and humidity readings on the screen.
 *
 * @param temperature The temperature value to display (in °C).
 * @param humidity The humidity value to display (in %).
 */
void display_show_readings(int temperature, int humidity);

#ifdef __cplusplus
}
#endif

#endif // DISPLAY_H