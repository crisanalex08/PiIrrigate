﻿using Microsoft.AspNetCore.Mvc;
using PiIrrigateServer.Models;
using PiIrrigateServer.Services;

namespace PiIrrigateServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
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

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest register)
        {
            try
            {
                var jwt = await userService.RegisterUser(register);
                return Ok(jwt);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message, e);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error registering user");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await userService.LoginUser(request);
            if (!result.Success) return Unauthorized(result.Message);

            return Ok(new { Token = result.Token, Message = result.Message });
        }

    }
}
