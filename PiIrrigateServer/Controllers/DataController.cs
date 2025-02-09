using Microsoft.AspNetCore.Mvc;
using PiIrrigateServer.Models;
using PiIrrigateServer.Services;

namespace PiIrrigateServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataController : ControllerBase
    {
        private readonly ILogger<DataController> logger;
        private readonly IDataService dataService;

        public DataController(ILogger<DataController> logger,
            IDataService dataService
            )
        {
            this.logger = logger;
            this.dataService = dataService;
        }


        [HttpGet(Name = "StoredData")]
        public async Task <IActionResult> GetStoredData()
        {
            return Ok(await dataService.GetAllStoredData());
        }

        //Add Data entry

        [HttpPost(Name = "AddData")]
        public async Task<IActionResult> AddData([FromBody] DataModel data)
        {
            await dataService.AddData(data);
            return Ok();
        }
    }
}
