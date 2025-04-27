using Microsoft.AspNetCore.Mvc;
using PiIrrigateServer.Managers;
using PiIrrigateServer.Models;
using PiIrrigateServer.Repositories;

namespace PiIrrigateServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DeviceController : ControllerBase
    {
        private readonly ILogger<UserManagementController> logger;
        private readonly IDeviceManager deviceManager;
        private readonly IDeviceRepository deviceRepository;
        public DeviceController(ILogger<UserManagementController> logger, IDeviceManager deviceManager, IDeviceRepository deviceRepository)
        {
            this.logger = logger;
            this.deviceManager = deviceManager;
            this.deviceRepository = deviceRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterDevice([FromBody] RegisterDeviceRequest register)
        {
            try
            {
                // Check if a device with the same MAC address already exists  
                var existingDevice = await deviceRepository.GetByMacAsync(register.Mac);
                if (existingDevice != null)
                {
                    return Conflict("A device with the same MAC address already exists.");
                }

                // Create a new device object  
                var newDevice = new Device
                {
                    ZoneId = Guid.NewGuid(),
                    Mac = register.Mac,
                    Name = register.Name,
                    Location = register.Location,
                    Owner = register.Owner,
                    Description = register.Description,
                    IsRegistered = false
                };

                // Save the device to the local database  
                var created = await deviceRepository.CreateAsync(newDevice);
                if (!created)
                {
                    return StatusCode(500, "Failed to save the device to the database.");
                }

                // Create the IoT Hub device  
                var iotDeviceId = await deviceManager.CreateIotDevice();
                if (string.IsNullOrEmpty(iotDeviceId))
                {
                    return StatusCode(500, "Failed to create the IoT Hub device.");
                }

                // Retrieve the connection string for the IoT Hub device  
                var connectionString = await deviceManager.GetDeviceConnectionString(iotDeviceId);
                if (string.IsNullOrEmpty(connectionString))
                {
                    return StatusCode(500, "Failed to retrieve the IoT Hub device connection string.");
                }

                // Update the device as registered  
                newDevice.IsRegistered = true;
                var updated = await deviceRepository.UpdateAsync(newDevice);
                if (!updated)
                {
                    return StatusCode(500, "Failed to update the device registration status.");
                }

                // Return success response with the connection string  
                return Ok(new { ConnectionString = connectionString });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while registering the device.");
                return StatusCode(500, "An internal server error occurred.");
            }
        }
    }
}