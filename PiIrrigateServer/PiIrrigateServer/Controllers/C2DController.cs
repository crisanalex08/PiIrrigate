﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PiIrrigateServer.Models;
using PiIrrigateServer.Repositories;
using PiIrrigateServer.Services;

namespace PiIrrigateServer.Controllers
{
    [ApiController]
    public class C2DController : ControllerBase
    {
        private readonly ILogger<C2DController> logger;
        private readonly C2DMessageSenderManager c2DMessageSenderManager;
        private readonly IZoneRepository zones;
        private string serviceConnectionString;

        public C2DController(ILogger<C2DController> logger, C2DMessageSenderManager c2DMessageSenderManager,
            IOptions<IoTHubConfiguraiton> options,
            IZoneRepository zones)
        {
            this.logger = logger;
            this.c2DMessageSenderManager = c2DMessageSenderManager;
            this.zones = zones;
        }

        [HttpPost("c2d/sendMessage")]
        public async Task<IActionResult> SendC2DMessage(C2DMessageRequest c2DMessageRequest)
        {
            try
            {
                using (var sender = c2DMessageSenderManager.GetC2DMessageSender())
                {
                    var messageId = await sender.SendC2DMessage(c2DMessageRequest.ZoneId.ToString(), c2DMessageRequest.methodCall);
                    logger.LogInformation("Message sent");
                    return Ok(new { MessageId = messageId });
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return Problem(e.Message);
            }
        }
    }
}
