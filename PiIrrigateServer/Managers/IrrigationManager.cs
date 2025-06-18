using PiIrrigateServer.Models;

namespace PiIrrigateServer.Managers
{
    public class IrrigationManager
    {
        private readonly ILogger<IrrigationManager> logger;
        private readonly IServiceScopeFactory serviceScopeFactory;

        // Thresholds and configuration parameters
        private readonly double MinTemperature = -20;
        private readonly double MaxTemperature = 60;
        private readonly double MinHumidity = 0;
        private readonly double MaxHumidity = 100;
        private readonly double MinSoilMoisture = 0;
        private readonly double MaxSoilMoisture = 100;
        private readonly double MaxRainfall = 100;
        private readonly double MaxWaterTemperature = 40;

        // Decision logic thresholds
        private readonly double IrrigationSoilMoistureThreshold = 40;
        private readonly double IrrigationRainfallThreshold = 20;
        private readonly double OverwateringSoilMoistureThreshold = 80;
        private readonly double UnderwateringSoilMoistureThreshold = 30;
        private readonly double RainfallHighThreshold = 50;

        public IrrigationManager(ILogger<IrrigationManager> logger, IServiceScopeFactory serviceScopeFactory)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
        }

        public void ProcessSensorReading(SensorReading reading)
        {
            if (!IsValid(reading))
            {
                logger.LogWarning("Invalid sensor reading at {Timestamp} from device {Mac}", reading.Timestamp, reading.Mac);
                return;
            }

            // Rule 1: Irrigation Trigger
            if (reading.SoilMoisture < IrrigationSoilMoistureThreshold && reading.Rainfall < IrrigationRainfallThreshold)
            {
                logger.LogInformation("Irrigation needed in zone {ZoneId}. Soil moisture: {SoilMoisture}%, Rainfall: {Rainfall}%",
                    reading.ZoneId, reading.SoilMoisture, reading.Rainfall);
            }

            // Rule 2: Overwatering Detection
            if (reading.SoilMoisture > OverwateringSoilMoistureThreshold && reading.WaterFlow > 0)
            {
                logger.LogWarning("Overwatering detected in zone {ZoneId}. Soil moisture: {SoilMoisture}%, Water flow: {WaterFlow}",
                    reading.ZoneId, reading.SoilMoisture, reading.WaterFlow);
            }

            // Rule 3: Underwatering / Dry Alert
            if (reading.SoilMoisture < UnderwateringSoilMoistureThreshold && reading.WaterFlow == 0)
            {
                logger.LogWarning("Dry zone alert in zone {ZoneId}. Soil moisture: {SoilMoisture}%, No water flow.",
                    reading.ZoneId, reading.SoilMoisture);
            }

            // Rule 4: Rainfall Reaction
            if (reading.Rainfall > RainfallHighThreshold && reading.WaterFlow > 0)
            {
                logger.LogWarning("Suspicious irrigation during rain in zone {ZoneId}. Rainfall: {Rainfall}%, Water flow: {WaterFlow}",
                    reading.ZoneId, reading.Rainfall, reading.WaterFlow);
            }
        }

        private bool IsValid(SensorReading reading)
        {
            return
             reading.Temperature >= MinTemperature && reading.Temperature <= MaxTemperature &&
             reading.Humidity >= MinHumidity && reading.Humidity <= MaxHumidity &&
             reading.SoilMoisture >= MinSoilMoisture && reading.SoilMoisture <= MaxSoilMoisture &&
             reading.Rainfall >= 0 && reading.Rainfall <= MaxRainfall &&
             reading.WaterTemperature >= 0 && reading.WaterTemperature <= MaxWaterTemperature &&
             reading.WaterFlow >= 0;
        }
    }
}
