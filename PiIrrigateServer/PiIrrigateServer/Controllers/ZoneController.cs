using Microsoft.AspNetCore.Mvc;
using PiIrrigateServer.Exceptions;
using PiIrrigateServer.Managers;
using PiIrrigateServer.Models;
using PiIrrigateServer.Repositories;
using PiIrrigateServer.Services;

namespace PiIrrigateServer.Controllers
{
    [ApiController]
    public class ZoneController : ControllerBase
    {
        private readonly ILogger<ZoneController> logger;
        private readonly IZoneRepository zoneRepository;
        private readonly IiotDeviceManager iotDeviceManager;
        private readonly C2DMessageSenderManager c2DMessageSenderManager;

        public ZoneController(ILogger<ZoneController> logger, IZoneRepository zoneRepository, IiotDeviceManager iotDeviceManager, C2DMessageSenderManager c2DMessageSenderManager)
        {
            this.logger = logger;
            this.zoneRepository = zoneRepository;
            this.iotDeviceManager = iotDeviceManager;
            this.c2DMessageSenderManager = c2DMessageSenderManager;
        }

        [HttpPost("zone/create")]
        public async Task<IActionResult> CreateZone([FromBody] CreateZoneRequest register)
        {
            try
            {
                Zone newZone = new()
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

                return Ok(new { ZoneId = newZone.ZoneId, ConnectionString = newZone.ConnectionString });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating zone");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("zone/{userId}/zones")]
        public async Task<IActionResult> GetAllZones(Guid userId)
        {
            try
            {
                return Ok(await zoneRepository.GetAllByUserId(userId));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating zone");
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet("zone/{zoneId}/connection-string")]
        public async Task<IActionResult> GetZoneConnectionString(Guid zoneId)
        {
            try
            {
                Zone zone = await zoneRepository.GetZoneById(zoneId);                
                return Ok(zone.ConnectionString);
            }
            catch (ZoneNotFoundException ex)
            {
                logger.LogWarning(ex, "Zone with ID {ZoneId} not found", zoneId);
                return StatusCode(404, "Zone not found");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching the connectionstring");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("zone/activate")]
        public async Task<IActionResult> ActivateZone(ActivateZoneRequest request)
        {
            try
            {
                var zone = await zoneRepository.GetZoneByName(request.ActivationCode);

                foreach (var device in zone.Devices)
                {
                    device.IsRegistered = true;
                }
                await zoneRepository.UpdateZone(zone.ZoneId, request.ZoneName, request.UserId);

                var messageSender = c2DMessageSenderManager.GetC2DMessageSender();

                var methodCall = new C2DMethodCall()
                {
                    DeviceId = zone.ZoneId.ToString(),
                    Method = "ActivateDevice",
                    Params = new[]
                    {
                            new MethodParams { Name = "RefreshInterval", Value = request.RefreshInterval.ToString() }
                        }
                };
                await messageSender.SendC2DMessage(zone.ZoneId.ToString(), methodCall);
                return Ok("Zone activated");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating zone");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
