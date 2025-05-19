using Microsoft.Azure.Devices;
using PiIrrigateServer.Models;
using System.Text;
using System.Text.Json;

namespace PiIrrigateServer.Services
{
    public class C2DMessageSenderManager
    {
        private readonly ILogger<C2DMessageSenderManager> logger;

        public C2DMessageSenderManager(ILogger<C2DMessageSenderManager> logger)
        {
            this.logger = logger;
        }

        public C2DMessageSender GetC2DMessageSender(string deviceConnectionString)
        {
            return new C2DMessageSender(deviceConnectionString);
        }
    }

    public class C2DMessageSender : IDisposable
    {
        private readonly ServiceClient _serviceClient;
        private bool _disposed = false; // To detect redundant calls

        public C2DMessageSender(string deviceConnectionString)
        {
            _serviceClient = ServiceClient.CreateFromConnectionString(deviceConnectionString);
        }

        public async Task<string> SendC2DMessage(string hubDeviceId, C2DMethodCall methodCall)
        {
            var messagePayload = JsonSerializer.Serialize(new
            {
                methodCall.Method,
                methodCall.Params
            });

            var message = new Message(Encoding.UTF8.GetBytes(messagePayload))
            {
                ContentType = "application/json",
                ContentEncoding = "utf-8"
            };

            try
            {
                TimeSpan operationTimeout = TimeSpan.FromSeconds(10);
                await _serviceClient.SendAsync(hubDeviceId, message, operationTimeout);
                return message.MessageId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); // Suppress finalization to optimize garbage collection
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources
                    _serviceClient?.Dispose();
                }

                _disposed = true;
            }
        }
    }
}
