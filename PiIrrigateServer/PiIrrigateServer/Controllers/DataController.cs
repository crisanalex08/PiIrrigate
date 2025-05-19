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

        [HttpGet("data/{zoneId}/getData")]
        public async Task<IActionResult> GetData(DateTime from, DateTime to, Guid zoneId)
        {
            try
            {
                var data = await dataManager.GetTimedZoneData(from, to, zoneId);
                return Ok(data);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return Problem(e.Message);
            }
        }

        [HttpGet("data/{zoneId}/{deviceId}/getData")]
        public async Task<IActionResult> GetDeviceData(DateTime from, DateTime to, Guid zoneId, string deviceId)
        {
            try
            {
                var data = await dataManager.GetTimedDeviceData(from, to, zoneId, deviceId);
                return Ok(data);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return Problem(e.Message);
            }
        }

        [HttpGet("data/{zoneId}/getAllData")]
        public async Task<IActionResult> GetData(Guid zoneId)
        {
            try
            {
                var data = await dataManager.GetAllZoneData(zoneId);
                return Ok(data);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return Problem(e.Message);
            }
        }

        [HttpGet("data/{zoneId}/{deviceId}/getAllData")]
        public async Task<IActionResult> GetDeviceData(Guid zoneId, string deviceId)
        {
            try
            {
                var data = await dataManager.GetAllDeviceData(zoneId, deviceId);
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
