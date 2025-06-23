using Microsoft.AspNetCore.Cors;
using Microsoft.Azure.Devices;
using PiIrrigateServer.Models;
using PiIrrigateServer.Repositories;
using PiIrrigateServer.Services;

namespace PiIrrigateServer.Managers
{
    public class IrrigationManager
    {
        private readonly IServiceScope scope;
        private readonly ILogger<IrrigationManager> logger;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly C2DMessageSender c2DMessageSender;
        private readonly ConfigurationService configService;
        private readonly PiIrrigateServer.Models.Configuration config;

        // Thresholds and configuration parameters
        private readonly double MinTemperature = -20;
        private readonly double MaxTemperature = 60;
        private readonly double MinHumidity = 0;
        private readonly double MaxHumidity = 100;
        private readonly double MinSoilMoisture = 0;
        private readonly double MaxSoilMoisture = 100;
        private readonly double MaxRainfall = 100;
        private readonly double MaxWaterTemperature = 40;
    

        public IrrigationManager(ILogger<IrrigationManager> logger, IServiceScopeFactory serviceScopeFactory, ConfigurationService config, C2DMessageSenderManager c2DMessageSenderManager,
            IServiceScopeFactory scopeFactory)
        {
            this.scope = serviceScopeFactory.CreateScope();
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
            this.c2DMessageSender = c2DMessageSenderManager.GetC2DMessageSender();
            this.configService = config;
        }

        public async Task ProcessSensorReading(SensorReading reading)
        {
            var config = configService.GetConfiguration();
            if(!config.enableAutomation)
            {
                await Task.CompletedTask;
            }

            // Rule 1: Irrigation Trigger
            if (reading.SoilMoisture < config.soilMoistureLowThreshold && reading.Rainfall < config.rainDropHighThreshold && !reading.Watering)
            {
                logger.LogInformation("Irrigation needed in zone {ZoneId}. Soil moisture: {SoilMoisture}%, Rainfall: {Rainfall}%",
                    reading.ZoneId, reading.SoilMoisture, reading.Rainfall);

                await turnIrrigationOn(reading.Mac);
            }

            // Rule 2: Overwatering Detection
            if (reading.SoilMoisture > config.soilMoistureHighThreshold && reading.Watering)
            {
                logger.LogWarning("Overwatering detected in zone {ZoneId}. Soil moisture: {SoilMoisture}%, Water flow: {WaterFlow}",
                    reading.ZoneId, reading.SoilMoisture, reading.WaterFlow);

                await turnIrrigationOff(reading.Mac);
            }

            // Rule 3: Underwatering / Dry Alert
            if (reading.SoilMoisture < config.soilMoistureLowThreshold && reading.WaterFlow == 0 && !reading.Watering)
            {
                logger.LogWarning("Dry zone alert in zone {ZoneId}. Soil moisture: {SoilMoisture}%, No water flow.",
                    reading.ZoneId, reading.SoilMoisture);

                await turnIrrigationOn(reading.Mac);
            }

            // Rule 4: Rainfall Reaction
            if (reading.Rainfall > config.rainDropLowThreshold && reading.WaterFlow > 0 && !reading.Watering)
            {
                logger.LogWarning("Suspicious irrigation during rain in zone {ZoneId}. Rainfall: {Rainfall}%, Water flow: {WaterFlow}",
                    reading.ZoneId, reading.Rainfall, reading.WaterFlow);

                await turnIrrigationOff(reading.Mac);
            }

            logger.LogInformation(reading.ToString());
        }

        public async Task turnIrrigationOn(string deviceId)
        {
            var repo = scope.ServiceProvider.GetRequiredService<IDeviceRepository>();
            var device = await repo.GetByMacAsync(deviceId);

            var methodCall = new Models.C2DMethodCall
            {
                Method = "relayOn",
                DeviceId = deviceId
            };


            var messageId = await c2DMessageSender.SendC2DMessage(device.ZoneId.ToString(), methodCall);
        }
        public async Task turnIrrigationOff(string deviceId)
        {
            var repo = scope.ServiceProvider.GetRequiredService<IDeviceRepository>();
            var device = await repo.GetByMacAsync(deviceId);

            var methodCall = new Models.C2DMethodCall
            {
                Method = "relayOff",
                DeviceId = deviceId
            };


            var messageId = await c2DMessageSender.SendC2DMessage(device.ZoneId.ToString(), methodCall);
        }
    }
}
