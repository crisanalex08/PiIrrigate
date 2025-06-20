using PiIrrigateServer.Models;

namespace PiIrrigateServer.Services
{
    public class ConfigurationService
    {
        public Configuration Configuration
            = new Configuration()
            {
                enableNotifications = true,
                irrigationRunning = false,
                soilMoistureHighThreshold = 70,
                soilMoistureLowThreshold = 30,
                rainDropLowThreshold = 10,
                rainDropHighThreshold = 50,
                waterFlowLowThreshold = 5,
                waterFlowHighThreshold = 20,
                waterTemperatureHighThreshold = 30,
                waterTemperatureLowThreshold = 10
            };

        public Configuration GetConfiguration()
        {
            return Configuration;
        }
    }
}
