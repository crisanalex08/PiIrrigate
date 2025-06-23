#include <WiFi.h>
#include <time.h>

const char* ssid     = "MERCUSYS_3B64";
const char* password = "17082002";

const char* ntpServer = "pool.ntp.org";
const long  gmtOffset_sec = 0;           // GMT
const int   daylightOffset_sec = 0;      // No DST

void initMyTime() {
  Serial.println("Initializing WiFi and NTP...");
  Serial.begin(115200);
  WiFi.begin(ssid, password);
  Serial.println("Initiated WiFi and NTP...");

  int retries = 0;
  const int maxRetries = 3;
  while (WiFi.status() != WL_CONNECTED && retries < maxRetries) {
    delay(500);
    Serial.print(".");
    retries++;
  }
  if (WiFi.status() != WL_CONNECTED) {
    Serial.println("\nWiFi connection failed after 3 attempts.");
    return;
  }
  Serial.println("\nWiFi connected.");

  // Initialize NTP
  configTime(gmtOffset_sec, daylightOffset_sec, ntpServer);
}

time_t getEpochTime() {
  struct tm timeinfo;
  if (getLocalTime(&timeinfo)) {
    return mktime(&timeinfo);
  } else {
    Serial.println("Failed to obtain time");
    return 0;
  }
}