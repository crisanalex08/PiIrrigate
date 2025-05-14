using Microsoft.AspNetCore.Mvc;
using PiIrrigateServer.Managers;

namespace PiIrrigateServer.Controllers
{
    [ApiController]
    public class DataController
    {
        private readonly ILogger<DataController> logger;

        public DataController(ILogger<DataController> logger, 
            IDataManager dataManager)
        {
            this.logger = logger;
        }

        [HttpGet]

    }
}
