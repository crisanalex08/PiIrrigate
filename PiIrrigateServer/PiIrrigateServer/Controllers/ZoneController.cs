using Microsoft.AspNetCore.Mvc;
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

        public ZoneController(ILogger<ZoneController> logger, IZoneRepository zoneRepository)
        {
            this.logger = logger;
            this.zoneRepository = zoneRepository;
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
