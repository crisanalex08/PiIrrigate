using Microsoft.AspNetCore.Mvc;
using PiIrrigateServer.Models;
using PiIrrigateServer.Services;

namespace PiIrrigateServer.Controllers
{
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private readonly ILogger<UserManagementController> logger;
        private readonly IUserService userService;

        public UserManagementController(ILogger<UserManagementController> logger,
            IUserService userService)
        {
            this.logger = logger;
            this.userService = userService;
        }

        [HttpPost("user/register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest register)
        {
            try
            {
                var authResult = await userService.RegisterUser(register);
                return Ok(authResult);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message, e);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error registering user");
            }
        }

        [HttpPost("user/login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var authResult = await userService.LoginUser(request);
            if (!authResult.Success) return Unauthorized(authResult.Message);

            return Ok(authResult);
        }

    }
}
