using Microsoft.AspNetCore.Mvc;
using PiIrrigateServer.Models;
using PiIrrigateServer.Services;

namespace PiIrrigateServer.Controllers
{
    [ApiController]
    public class ConfigurationController: ControllerBase
    {
        private readonly ConfigurationService configurationService;

        public ConfigurationController(ConfigurationService configurationService)
        {
            this.configurationService = configurationService;
        }

        [HttpGet("api/configuration")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var configuration = configurationService.GetConfiguration();
                return Ok(configuration);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [HttpPost("api/configuration")]
        public async Task<IActionResult> UpdateConfiguration(Configuration configuration)
        {
            try
            {
                if (configuration == null)
                {
                    return BadRequest("Configuration cannot be null.");
                }
                configurationService.Configuration = configuration;
                return Ok(configuration);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }

        }
    }
}
