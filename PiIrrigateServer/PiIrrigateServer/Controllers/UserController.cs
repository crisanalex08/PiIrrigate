using Microsoft.AspNetCore.Mvc;
using PiIrrigateServer.Models;
using PiIrrigateServer.Repositories;
using PiIrrigateServer.Services;

namespace PiIrrigateServer.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> logger;
        private readonly IUserRepository userRepository;
        private readonly IJwtService jwtService;
        private readonly IPasswordHasher passwordHasher;

        public UserController(ILogger<UserController> logger, 
            IUserRepository userRepository, 
            IJwtService jwtService, 
            IPasswordHasher passwordHasher)
        {
            this.logger = logger;
            this.userRepository = userRepository;
            this.jwtService = jwtService;
            this.passwordHasher = passwordHasher;
        }

        [HttpPost("user/register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            try
            {
                var existingUser = await userRepository.GetByEmailAsync(registerRequest.Email);
                if (existingUser != null)
                {
                    return Problem("Email is already used", statusCode: 400);
                }

                var user = new User()
                {
                    FullName = registerRequest.FullName,
                    Email = registerRequest.Email,
                    Role = Enums.UserRole.User,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                string hash = passwordHasher.HashPassword(user, registerRequest.Password);
                user.PasswordHash = hash;

                await userRepository.CreateAsync(user);

                var token = jwtService.GenerateJwtToken(user);

                return Ok(new AuthResult { Success = true, Token = token, UserId = user.Id, Message = "User registered successfully" });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpPost("user/login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await userRepository.GetByEmailAsync(request.Email);
            if (user == null)
            {
                return Problem("Invalid credentials", statusCode: 401);
            }

            var passwordVerification = passwordHasher.VerifyPassword(user, user.PasswordHash, request.Password);
            if (passwordVerification == false)
            {
                return Problem("Invalid credentials", statusCode: 401);
            }

            var token = jwtService.GenerateJwtToken(user);

            return Ok(new AuthResult { Success = true, Token = token, UserId = user.Id, Message = "Login successful" });
        }

    }
}
