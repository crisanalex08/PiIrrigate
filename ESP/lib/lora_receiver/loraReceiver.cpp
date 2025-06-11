#include "LoraReceiver.h"
#include <LoRa.h>
#include <Arduino.h>
#include <sensor_data.h>

void LoraReceiver::init()
{
    // Initialize the LoRa module
    if (!LoRa.begin(433.0))
    { // Set frequency to 433 MHz
        Serial.println("LoRa initialization failed!");
        while (1)
            ; // Halt execution if LoRa initialization fails
    }
    Serial.println("LoRa Receiver initialized");
}

bool LoraReceiver::receiveSensorData(LoRaPacket &outPacket)
{
    int packetSize = LoRa.parsePacket(); // Check if a packet is available
    if (packetSize > 0)
    {
        Serial.println("Received a complete LoRa packet");
        // Read the binary packet directly into the struct
        LoRa.readBytes(reinterpret_cast<uint8_t *>(&outPacket), sizeof(LoRaPacket));

        // Send acknowledgment
        LoRa.beginPacket();
        LoRa.print("ACK");
        LoRa.endPacket();

        return true;
    }
    return false;
}

void LoraReceiver::receiveLoRaCommand(LoRaCommand &command)
{
    int packetSize = 0;
    unsigned long startTime = millis();
    // Wait up to 2 seconds for a packet
    while ((packetSize = LoRa.parsePacket()) <= 0 && (millis() - startTime < 2000))
    {
        delay(10); // Small delay to avoid busy loop
    }

    if (packetSize > 0)
    {
        // Read the command data
        String commandString = LoRa.readStringUntil('\n'); // Read until newline character
        Serial.println("Received command: " + commandString);

        // Parse the command string into the LoRaCommand structure
        // C:methodName,param1=value1,param2=value2
        int pos = commandString.indexOf(':');
        if (pos != -1)
        {
            // Skip the leading 'C:' and extract the method name
            int methodStart = pos + 1;
            int methodEnd = commandString.indexOf(',', methodStart);
            if (methodEnd == -1)
                methodEnd = commandString.length();
            command.methodName = std::string(commandString.substring(methodStart, methodEnd).c_str());

            // Extract parameters string (if any)
            if (methodEnd < commandString.length())
            {
                String paramsString = commandString.substring(methodEnd + 1);
                while (paramsString.length() > 0)
                {
                    int commaPos = paramsString.indexOf(',');
                    String param = (commaPos == -1) ? paramsString : paramsString.substring(0, commaPos);
                    int eqPos = param.indexOf('=');
                    if (eqPos != -1)
                    {
                        LoRaCommandParameter parameter;
                        parameter.parameterName = std::string(param.substring(0, eqPos).c_str());
                        parameter.parameterValue = std::string(param.substring(eqPos + 1).c_str());
                        command.parameters.push_back(parameter);
                    }
                    if (commaPos == -1)
                        break;
                    paramsString = paramsString.substring(commaPos + 1);
                }
            }
        }

        LoRa.beginPacket();
        LoRa.print("ACK");
        LoRa.endPacket();
    }
}
