using Microsoft.Azure.Devices;
using Microsoft.Extensions.Options;
using PiIrrigateServer.Models;

namespace PiIrrigateServer.Managers
{
    public interface IiotDeviceManager
    {
        public Task<bool> CreateIotDevice(string zoneId);
        public Task<string> GetDeviceConnectionString(string zoneId);
    }
    public class IotDeviceManager : IiotDeviceManager
    {
        private readonly RegistryManager registryManager;
        private string IoTHubHostName;
        public IotDeviceManager(IOptions<IoTHubConfiguraiton> options)
        {
            registryManager = RegistryManager.CreateFromConnectionString(options.Value.ConnectionString);
            IoTHubHostName = options.Value.ConnectionString.Split(';')[0].Split('=')[1];
        }
        public async Task<bool> CreateIotDevice(string zoneId)
        {
            try
            {
                var existingDevice = await registryManager.GetDeviceAsync(zoneId);
                if (existingDevice != null)
                {
                    // Device already exists, no need to create a new one  
                    return false;
                }

                var device = new Microsoft.Azure.Devices.Device(zoneId);
                await registryManager.AddDeviceAsync(device);
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception (logging mechanism not shown here)  
                throw new InvalidOperationException($"Failed to create IoT device for ZoneId: {zoneId}", ex);
            }
        }

        public async Task<string> GetDeviceConnectionString(string zoneId)
        {
            var device = await registryManager.GetDeviceAsync(zoneId);

            if (device != null)
            {
                return $"HostName={IoTHubHostName};DeviceId={device.Id};SharedAccessKey={device.Authentication.SymmetricKey.PrimaryKey}";

            }
            return string.Empty;
        }
    }
}
