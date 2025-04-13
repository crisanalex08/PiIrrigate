#include "dht.h"
#include <u8g2.h>

// Initialize the U8g2 library for the T-Beam screen
u8g2_t u8g2;

void init_screen() {
    // Initialize the screen (I2C example, adjust pins as needed)
    u8g2_Setup_ssd1306_i2c_128x64_noname_f(
        &u8g2, U8G2_R0, 
        u8x8_gpio_and_delay_esp32, 
        u8x8_byte_hw_i2c
    );
    u8g2_InitDisplay(&u8g2);
    u8g2_SetPowerSave(&u8g2, 0);  // Turn on the display
    u8g2_ClearBuffer(&u8g2);
}

void display_readings(int temperature, int humidity) {
    char temp_str[16];
    char hum_str[16];

    // Format the temperature and humidity strings
    snprintf(temp_str, sizeof(temp_str), "Temp: %d°C", temperature);
    snprintf(hum_str, sizeof(hum_str), "Hum: %d%%", humidity);

    // Clear the screen and draw the new readings
    u8g2_ClearBuffer(&u8g2);
    u8g2_SetFont(&u8g2, u8g2_font_ncenB08_tr);  // Set font
    u8g2_DrawStr(&u8g2, 0, 20, temp_str);       // Draw temperature
    u8g2_DrawStr(&u8g2, 0, 40, hum_str);        // Draw humidity
    u8g2_SendBuffer(&u8g2);                     // Send buffer to display
}