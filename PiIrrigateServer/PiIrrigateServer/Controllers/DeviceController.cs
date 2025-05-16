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
        private readonly IiotDeviceManager deviceManager;
        private readonly IDeviceRepository deviceRepository;
        private readonly IZoneRepository zoneRepository;

        public DeviceController(ILogger<UserManagementController> logger, IiotDeviceManager deviceManager, IDeviceRepository deviceRepository, IZoneRepository zoneRepository)
        {
            this.logger = logger;
            this.deviceManager = deviceManager;
            this.deviceRepository = deviceRepository;
            this.zoneRepository = zoneRepository;
        }

        [HttpPost("api/register")]
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
                    ZoneId = register.ZoneId,
                    Mac = register.Mac,
                    Name = register.Name,
                    Location = register.Location,
                    Description = register.Description,
                    IsRegistered = false
                };

                // Save the device to the local database  
                var created = await deviceRepository.CreateAsync(newDevice);
                if (!created)
                {
                    return StatusCode(500, "Failed to save the device to the database.");
                }

                // Update the device as registered  
                newDevice.IsRegistered = true;
                var updated = await deviceRepository.UpdateAsync(newDevice);
                if (!updated)
                {
                    return StatusCode(500, "Failed to update the device registration status.");
                }

                // Return success response with the connection string  
                return Ok(new { deviceId = newDevice.Mac });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while registering the device.");
                return StatusCode(500, "An internal server error occurred.");
            }
        }

        [HttpGet("api/getAllDevice")]
        public async Task<IActionResult> GetDevices() {
            try
            {
                var devices = await deviceRepository.GetAllAsync();
                return Ok(devices);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return Problem(e.Message);
            }
        } 
    }
}