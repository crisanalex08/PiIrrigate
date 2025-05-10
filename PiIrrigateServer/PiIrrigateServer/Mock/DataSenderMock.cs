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

            while (!cancellationToken.IsCancellationRequested)
            {
                // Generate mock sensor data
                var mockData = new SensorReading
                {
                    ZoneId = Guid.NewGuid(),
                    Mac = Guid.NewGuid().ToString(),
                    Temperature = random.Next(15, 35), // Random temperature between 15°C and 35°C
                    Humidity = random.Next(30, 80),   // Random humidity between 30% and 80%
                    Timestamp = DateTime.UtcNow
                };

                // Send the mock data to all connected clients
                await _hubContext.Clients.All.SendAsync("sendLiveDataToZone", mockData);
                logger.LogInformation($"Data sent {mockData}");
                // Wait for 1 second before sending the next update
                await Task.Delay(1000, cancellationToken);
            }
        }
    }
}
