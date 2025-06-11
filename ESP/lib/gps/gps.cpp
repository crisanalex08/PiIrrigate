#include "gps.h"
#include <HardwareSerial.h>
#include <string.h>
#include <stdlib.h>

GpsModule::GpsModule(int uart_num, int tx_pin, int rx_pin, uint32_t baud_rate)
    : _uart_num(uart_num), _tx_pin(tx_pin), _rx_pin(rx_pin), _baud_rate(baud_rate),
      _latitude(0.0), _longitude(0.0), _hasFix(false)
{}

void GpsModule::init() {
    HardwareSerial* serial = new HardwareSerial(_uart_num);
    serial->begin(_baud_rate, SERIAL_8N1, _rx_pin, _tx_pin);
}

void GpsModule::update() {
    HardwareSerial* serial = new HardwareSerial(_uart_num);
    static char nmeaSentence[128];
    static int idx = 0;

    while (serial->available()) {
        char c = serial->read();
        if (c == '\n' || idx >= 127) {
            nmeaSentence[idx] = '\0';
            parseNmeaSentence(nmeaSentence);
            idx = 0;
        } else if (c != '\r') {
            nmeaSentence[idx++] = c;
        }
    }
}

double GpsModule::getLatitude() const {
    return _latitude;
}

double GpsModule::getLongitude() const {
    return _longitude;
}

bool GpsModule::hasFix() const {
    return _hasFix;
}

// Helper to convert NMEA lat/lon to decimal degrees
static double nmeaToDecimal(const char* nmea, const char* dir) {
    if (!nmea || !*nmea) return 0.0;
    double deg = atof(nmea);
    int d = (dir[0] == 'N' || dir[0] == 'S') ? 2 : 3;
    double degrees = (int)(deg / pow(10, strlen(nmea) - d));
    double minutes = deg - degrees * pow(10, strlen(nmea) - d);
    double result = degrees + minutes / 60.0;
    if (dir[0] == 'S' || dir[0] == 'W') result = -result;
    return result;
}

void GpsModule::parseNmeaSentence(const char* sentence) {
    // Only parse GGA or RMC for fix and position
    if (strstr(sentence, "$GPGGA") == sentence || strstr(sentence, "$GNGGA") == sentence) {
        // $GPGGA,time,lat,N,lon,E,fix,...
        char copy[128];
        strncpy(copy, sentence, sizeof(copy));
        char* tokens[15] = {0};
        char* p = strtok(copy, ",");
        int i = 0;
        while (p && i < 15) {
            tokens[i++] = p;
            p = strtok(NULL, ",");
        }
        if (i > 6 && tokens[2] && tokens[3] && tokens[4] && tokens[5] && tokens[6]) {
            _latitude = nmeaToDecimal(tokens[2], tokens[3]);
            _longitude = nmeaToDecimal(tokens[4], tokens[5]);
            _hasFix = atoi(tokens[6]) > 0;
        } else {
            _hasFix = false;
        }
    } else if (strstr(sentence, "$GPRMC") == sentence || strstr(sentence, "$GNRMC") == sentence) {
        // $GPRMC,time,status,lat,N,lon,E,...
        char copy[128];
        strncpy(copy, sentence, sizeof(copy));
        char* tokens[15] = {0};
        char* p = strtok(copy, ",");
        int i = 0;
        while (p && i < 15) {
            tokens[i++] = p;
            p = strtok(NULL, ",");
        }
        if (i > 6 && tokens[2] && tokens[3] && tokens[4] && tokens[5] && tokens[2][0] == 'A') {
            _latitude = nmeaToDecimal(tokens[3], tokens[4]);
            _longitude = nmeaToDecimal(tokens[5], tokens[6]);
            _hasFix = true;
        } else {
            _hasFix = false;
        }
    }
}