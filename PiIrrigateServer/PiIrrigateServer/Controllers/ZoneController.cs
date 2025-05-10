using Microsoft.AspNetCore.Mvc;
using PiIrrigateServer.Managers;
using PiIrrigateServer.Models;
using PiIrrigateServer.Repositories;

namespace PiIrrigateServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ZoneController :ControllerBase
    {
        private readonly ILogger<ZoneController> logger;
        private readonly IZoneRepository zoneRepository;
        private readonly IiotDeviceManager iotDeviceManager;

        public ZoneController(ILogger<ZoneController> logger, IZoneRepository zoneRepository, IiotDeviceManager iotDeviceManager)
        {
            this.logger = logger;
            this.zoneRepository = zoneRepository;
            this.iotDeviceManager = iotDeviceManager;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateZone([FromBody] CreateZoneRequest register)
        {
            try
            {
                Zone newZone = new Zone
                {
                    ZoneId = Guid.NewGuid(), // Generate a new GUID for the zone ID
                    Name = register.ZoneName,
                };

                // Create the IoT Hub device  

                if (!await iotDeviceManager.CreateIotDevice(newZone.ZoneId.ToString()))
                {
                    throw new Exception("Failed to create IoT device");
                }

                // Retrieve the connection string for the IoT Hub device  
                var connectionString = await iotDeviceManager.GetDeviceConnectionString(newZone.ZoneId.ToString());
                if (string.IsNullOrEmpty(connectionString))
                {
                    return StatusCode(500, "Failed to retrieve the IoT Hub device connection string.");
                }

                newZone.ConnectionString = connectionString;

                var created = await zoneRepository.CreateZone(newZone);
                if (!created)
                {
                    return Conflict("A zone with the same name already exists.");
                }

                return Ok(new { ZoneId = newZone.ZoneId });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating zone");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
