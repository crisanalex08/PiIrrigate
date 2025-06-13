using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PiIrrigateServer.Database;
using PiIrrigateServer.Models;
using PiIrrigateServer.Repositories;
using PiIrrigateServer.Services;
using PiIrrigateServer.SignalR;

namespace PiIrrigateServer.Managers
{
    public class IoTHubDataManager : IHostedService
    {
        private readonly ILogger<IoTHubDataManager> logger;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly IHubContext<LiveDataHub> hubContext;
        private readonly IDataRepository dataRepository;
        private readonly IDeviceRepository deviceRepository;
        private readonly string eventHubConnectionstring;
        private readonly string eventHubName;
        private readonly string consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;
        private EventHubConsumerClient consumer;

        public Dictionary<string, Guid> macZoneDict { get; set; }

        public IoTHubDataManager(ILogger<IoTHubDataManager> logger,
            IOptions<IoTHubConfiguraiton> options,
            IServiceScopeFactory serviceScopeFactory, 
            IHubContext<LiveDataHub> hubContext,
            IDataRepository dataRepository
            )
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.serviceScopeFactory = serviceScopeFactory;
            this.hubContext = hubContext;
            this.dataRepository = dataRepository;
            this.deviceRepository = deviceRepository;
            if (options == null) throw new ArgumentNullException(nameof(options));

            eventHubConnectionstring = options.Value.EventHubConnectionString
                ?? throw new ArgumentException("EventHubConnectionString cannot be null", nameof(options));
            eventHubName = options.Value.EventHubName
                ?? throw new ArgumentException("EventHubName cannot be null", nameof(options));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var task = ExecuteAsync(cancellationToken);
            if (task.IsCompleted)
            {
                logger.LogInformation("IoTHubDataManager started successfully.");
            }
            else
            {
                logger.LogWarning("IoTHubDataManager failed to start.");
            }
        }
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Starting IoTHubDataManager...");

                using var scope = serviceScopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                consumer = new EventHubConsumerClient(consumerGroup, eventHubConnectionstring, eventHubName);

                macZoneDict = await dbContext.Devices.ToDictionaryAsync(dbContext => dbContext.Mac, dbContext => dbContext.ZoneId, cancellationToken);

                while (!cancellationToken.IsCancellationRequested)
                {
                    await foreach (PartitionEvent partitionEvent in consumer.ReadEventsAsync(cancellationToken))
                    {
                        try
                        {
                            if(macZoneDict.Keys.Count == 0)
                            {
                                continue;
                            }
                            var data = partitionEvent.Data;
                            var sensorReading = GetReadingFromEventData(data);

                            // Store data
                            await dataRepository.StoreData(sensorReading);
                            
                            // Send live data
                            await hubContext.Clients.Group(sensorReading.ZoneId.ToString())
                                .SendAsync("ReceiveLiveData", sensorReading, cancellationToken);
                        }
                        catch (Exception e)
                        {
                            logger.LogError("The message could not be parsed");
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation("Event reading canceled.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while reading events.");
            }
        }

        private SensorReading GetReadingFromEventData(EventData eventData)
        {
            //body = C:2, ID:08F9E0CE7B8C, T:24, H:44, S:142, R:3134
            var pairs = eventData.EventBody.ToString().Split(", ");
            var data = new Dictionary<string, string>();

            foreach (var pair in pairs)
            {
                var keyValue = pair.Split(":");
                if (keyValue.Length == 2)
                {
                    data[keyValue[0]] = keyValue[1];
                }
            }

            data.TryGetValue("ID", out string mac);
            macZoneDict.TryGetValue(mac, out Guid zoneId);

            SensorReading sensorReading = new()
            {
                ZoneId = zoneId,
                Mac = mac,
                Timestamp = eventData.EnqueuedTime.UtcDateTime,
                Temperature = double.Parse(data["T"]),
                Humidity = double.Parse(data["H"]),
                SoilMoisture = double.Parse(data["S"]),
                Rainfall = double.Parse(data["R"])
            };

            return sensorReading;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Stopping IoTHubDataManager...");
            if (consumer != null)
            {
                await consumer.CloseAsync(cancellationToken);
                consumer = null;
            }
        }
    }
}
