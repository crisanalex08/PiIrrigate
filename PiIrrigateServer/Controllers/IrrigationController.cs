using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using PiIrrigateServer.Repositories;
using PiIrrigateServer.Services;
using System.Net;

namespace PiIrrigateServer.Controllers
{
    [ApiController]
    public class IrrigationController : ControllerBase
    {
        private readonly ILogger<IrrigationController> logger;
        private readonly IDeviceRepository deviceRepository;
        C2DMessageSender messageSender;
        public IrrigationController(C2DMessageSenderManager c2DMessageSenderManager, IDeviceRepository deviceRepository)
        {
            this.messageSender = c2DMessageSenderManager.GetC2DMessageSender();
            this.deviceRepository = deviceRepository;
        }

        [HttpPost("api/irrigation/start/{deviceId}")]
        public async Task<IActionResult> StartIrrigation(string deviceId)
        {
            var device = await this.deviceRepository.GetByMacAsync(deviceId);

            if (device == null)
            {
                return Problem("NotFound", statusCode: 404);
            }

            try
            {
                var methodCall = new Models.C2DMethodCall
                {
                    Method = "relayOn",
                    DeviceId = deviceId
                };

                
                var messageId = await messageSender.SendC2DMessage(device.ZoneId.ToString(), methodCall);
                return Ok(new { MessageId = messageId });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to send start irrigation command.");
                return Problem("Internal Server Error", statusCode: 500);
            }
        }

        [HttpPost("api/irrigation/stop/{deviceId}")]
        public async Task<IActionResult> StopIrrigation(string deviceId)
        {
            var device = await this.deviceRepository.GetByMacAsync(deviceId);

            if (device == null)
            {
                return Problem("NotFound", statusCode: 404);
            }

            try
            {
                var methodCall = new Models.C2DMethodCall
                {
                    Method = "relayOff",
                    DeviceId = deviceId
                };


                var messageId = await messageSender.SendC2DMessage(device.ZoneId.ToString(), methodCall);
                return Ok(new { MessageId = messageId });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to send start irrigation command.");
                return Problem("Internal Server Error", statusCode: 500);
            }
        }

    }
}
