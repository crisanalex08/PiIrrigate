using Microsoft.Azure.Devices;
using Microsoft.Extensions.Options;
using PiIrrigateServer.Models;

namespace PiIrrigateServer.Managers
{
    public interface IDeviceManager
    {
        public Task<string> CreateIotDevice();
        public Task<string> GetDeviceConnectionString(string deviceId);
    }
    public class DeviceManager : IDeviceManager
    {
        private readonly RegistryManager registryManager;
        private string IoTHubHostName;
        public DeviceManager(IOptions<IoTHubConfiguraiton> options)
        {
            registryManager = RegistryManager.CreateFromConnectionString(options.Value.ConnectionString);
            IoTHubHostName = options.Value.ConnectionString.Split(';')[0].Split('=')[1];
        }
        public async Task<string> CreateIotDevice()
        {
            var deviceId = Guid.NewGuid().ToString();
            var device = new Microsoft.Azure.Devices.Device(deviceId);

            await registryManager.AddDeviceAsync(device);
            return deviceId;
        }

        public async Task<string> GetDeviceConnectionString(string deviceId)
        {
            var device = await registryManager.GetDeviceAsync(deviceId);

            if (device != null)
            {
                return $"HostName={IoTHubHostName};DeviceId={device.Id};SharedAccessKey={device.Authentication.SymmetricKey.PrimaryKey}";

            }
            return string.Empty;
        }
    }
}
