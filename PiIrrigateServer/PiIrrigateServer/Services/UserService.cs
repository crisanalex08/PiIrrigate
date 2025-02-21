using PiIrrigateServer.Models;
using PiIrrigateServer.Repositories;

namespace PiIrrigateServer.Services
{
    public interface IUserService
    {
        Task<UserDto> GetUserByIdAsync(Guid userId);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<bool> UpdateUserProfileAsync(Guid userId, UpdateProfileRequest request);
        Task<bool> ChangeUserRoleAsync(Guid userId, string newRole);
        Task<AuthResult> RegisterUser(RegisterRequest register);
    }
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly ILogger<IUserService> logger;
        private readonly IJwtService jwtService;
        private readonly IPasswordHasher passwordHasher;

        public UserService(IUserRepository userRepository, 
            ILogger<IUserService> logger, IJwtService jwtService, IPasswordHasher passwordHasher)
        {
            this.userRepository = userRepository;
            this.logger = logger;
            this.jwtService = jwtService;
            this.passwordHasher = passwordHasher;
        }

        public Task<bool> ChangeUserRoleAsync(Guid userId, string newRole)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> GetUserByIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<AuthResult> RegisterUser(RegisterRequest registerRequest)
        {
            try
            {
                var existingUser = await userRepository.GetByEmailAsync(registerRequest.Email);
                if (existingUser != null)
                {
                    return new AuthResult { Success = false, Message = "Email already in use" };
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

                return new AuthResult { Success = true, Token = token, Message = "User registered successfully" };
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Task<bool> UpdateUserProfileAsync(Guid userId, UpdateProfileRequest request)
        {
            throw new NotImplementedException();
        }
    }
}

