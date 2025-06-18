using Microsoft.AspNetCore.Mvc;
using PiIrrigateServer.Repositories;

namespace PiIrrigateServer.Controllers
{
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly ILogger<DataController> logger;
        private readonly IDataRepository dataRepository;

        public DataController(ILogger<DataController> logger, 
            IDataRepository dataRepository)
        {
            this.logger = logger;
            this.dataRepository = dataRepository;
        }

        [HttpGet("data/{zoneId}/getData")]
        public async Task<IActionResult> GetData(DateTime from, DateTime to, Guid zoneId)
        {
            try
            {
                var data = await dataRepository.GetTimedZoneData(from, to, zoneId);
                return Ok(data);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return Problem(e.Message);
            }
        }

        [HttpGet("data/{zoneId}/getLast")]
        public async Task<IActionResult> GetLastData(DateTime from, DateTime to, Guid zoneId)
        {
            try
            {
                var data = await dataRepository.GetTimedZoneData(from, to, zoneId);
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
                var data = await dataRepository.GetTimedDeviceData(from, to, zoneId, deviceId);
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
                var data = await dataRepository.GetAllZoneData(zoneId);
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
                var data = await dataRepository.GetAllDeviceData(zoneId, deviceId);
                return Ok(data);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return Problem(e.Message);
            }
        }

        [HttpGet("data/{zoneId}/getLastData")]
        public async Task<IActionResult> GetLast(string zoneId)
        {
            try
            {
                var data = await dataRepository.GetLastAsync();
                return Ok(data);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return Problem(e.Message);
            }
        }

        [HttpDelete("data/allData")]
        public async Task<IActionResult> DeleteAllData()
        {
            try
            {
               await dataRepository.DeleteAll();
                return Ok();
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return Problem(e.Message);
            }
        }
    }
}
