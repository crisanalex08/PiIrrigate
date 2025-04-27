using Azure.Messaging.EventHubs.Consumer;
using Microsoft.Extensions.Options;
using PiIrrigateServer.Models;

namespace PiIrrigateServer.Managers
{
    public class IoTHubDataManager : IHostedService
    {
        private readonly ILogger<IoTHubDataManager> logger;
        private readonly string eventHubConnectionstring;
        private readonly string eventHubName;
        private readonly string consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;
        private EventHubConsumerClient consumer;

        public IoTHubDataManager(ILogger<IoTHubDataManager> logger,
            IOptions<IoTHubConfiguraiton> options)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            if (options == null) throw new ArgumentNullException(nameof(options));

            eventHubConnectionstring = options.Value.EventHubConnectionString
                ?? throw new ArgumentException("EventHubConnectionString cannot be null", nameof(options));
            eventHubName = options.Value.EventHubName
                ?? throw new ArgumentException("EventHubName cannot be null", nameof(options));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting IoTHubDataManager...");
            consumer = new EventHubConsumerClient(consumerGroup, eventHubConnectionstring, eventHubName);

            try
            {
                await foreach (PartitionEvent partitionEvent in consumer.ReadEventsAsync(cancellationToken))
                {
                    var data = partitionEvent.Data;
                    string body = data.EventBody.ToString();
                    logger.LogInformation($"Telemetry received: {body}");
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
