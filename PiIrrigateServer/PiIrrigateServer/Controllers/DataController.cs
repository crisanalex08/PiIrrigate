using Microsoft.AspNetCore.Mvc;
using PiIrrigateServer.Managers;

namespace PiIrrigateServer.Controllers
{
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly ILogger<DataController> logger;
        private readonly IDataManager dataManager;

        public DataController(ILogger<DataController> logger, 
            IDataManager dataManager)
        {
            this.logger = logger;
            this.dataManager = dataManager;
        }

        [HttpGet("api/{zoneId}getData")]
        public async Task<IActionResult> GetData(DateTime from, DateTime to, Guid zoneId)
        {
            try
            {
                var data = await dataManager.GetStoredData(from, to, zoneId);
                return Ok(data);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return Problem(e.Message);
            }
        }

        [HttpGet("api/{zoneId}getAllData")]
        public async Task<IActionResult> GetData( Guid zoneId)
        {
            try
            {
                var data = await dataManager.GetAllStoredData(zoneId);
                return Ok(data);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return Problem(e.Message);
            }
        }

    }
}
