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
                waterTemperatureLowThreshold = 10,
                refreshInterval = 10
            };

        public Configuration GetConfiguration()
        {
            return Configuration;
        }

        public void UpdateConfiguration(Configuration config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));

            Configuration.enableNotifications = config.enableNotifications;
            Configuration.irrigationRunning = config.irrigationRunning;
            Configuration.soilMoistureHighThreshold = config.soilMoistureHighThreshold;
            Configuration.soilMoistureLowThreshold = config.soilMoistureLowThreshold;
            Configuration.rainDropLowThreshold = config.rainDropLowThreshold;
            Configuration.rainDropHighThreshold = config.rainDropHighThreshold;
            Configuration.waterFlowLowThreshold = config.waterFlowLowThreshold;
            Configuration.waterFlowHighThreshold = config.waterFlowHighThreshold;
            Configuration.waterTemperatureHighThreshold = config.waterTemperatureHighThreshold;
            Configuration.waterTemperatureLowThreshold = config.waterTemperatureLowThreshold;
            Configuration.refreshInterval = config.refreshInterval;
        }
    }
}
