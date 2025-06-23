using Microsoft.AspNetCore.SignalR;
using PiIrrigateServer.Models;
using PiIrrigateServer.SignalR;

namespace PiIrrigateServer.Mock
{
    public class DataSenderMock
    {
        private readonly IHubContext<LiveDataHub> _hubContext;
        private readonly ILogger<DataSenderMock> logger;

        public DataSenderMock(IHubContext<LiveDataHub> hubContext, ILogger<DataSenderMock> logger)
        {
            _hubContext = hubContext;
            this.logger = logger;
        }

        public async Task StartSendingMockData(CancellationToken cancellationToken)
        {
            var random = new Random();

            var zoneIds = new[]
            {
                Guid.Parse("1f8e462f-1092-4f0b-86ae-0f765184e816"),
                Guid.Parse("b55dd4cb-b511-4cb5-a83d-8bc5766f9d42")
            };

            while (!cancellationToken.IsCancellationRequested)
            {
                foreach (var zoneId in zoneIds)
                {
                    // Generate mock sensor data for the current zone
                    var mockData = new SensorReading
                    {
                        ZoneId = zoneId,
                        Mac = Guid.NewGuid().ToString(),
                        Temperature = (int)(random.NextDouble() * (35 - 25) + 25), // Random temperature between 25°C and 35°C
                        Humidity = (int)(random.NextDouble() * (80 - 30) + 30),    // Random humidity between 30% and 80%
                        SoilMoisture = (int)(random.NextDouble() * 100),           // Random soil moisture between 0% and 200%
                        Rainfall = (int)(random.NextDouble() * 100),               // Random rainfall between 0mm and 100mm
                        WaterFlow = (int)(random.NextDouble() * 30),              // Random water flow between 0L/min and 100L/min
                        WaterTemperature = (int)(random.NextDouble() * (30 - 20) + 20), // Random water temperature between 10°C and 30°C
                        Longitude = (int)(random.NextDouble() * 180 - 90),         // Random longitude between -90 and 90
                        Latitude = (int)(random.NextDouble() * 360 - 180),         // Random latitude between -180 and 180
                        Timestamp = DateTime.UtcNow,
                        ReadingTime = DateTime.UtcNow.AddSeconds(random.Next(-3600, 3600)) // Random reading time within the last hour
                    };

                    // Send the mock data to the specific zone group
                    await _hubContext.Clients.Group(zoneId.ToString())
                        .SendAsync("ReceiveLiveData", mockData, cancellationToken);

                    logger.LogInformation($"Data sent to Zone {zoneId}: {mockData}");
                }

                // Wait for 2 seconds before sending the next update
                await Task.Delay(2000, cancellationToken);
            }
        }
    }
}
