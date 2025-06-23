using Microsoft.Extensions.Configuration;
using PiIrrigateServer.Enums;
using PiIrrigateServer.Models;
using PiIrrigateServer.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Xunit;

namespace PiIrrigateServer.Tests
{
    [TestClass]
    public class JwtServiceTests
    {
        private readonly IJwtService _jwtService;

        public JwtServiceTests()
        {
            // Arrange mock configuration
            var inMemorySettings = new Dictionary<string, string> {
                {"Jwt:Secret", "ThisIsASecretKeyForTestingPurposes123!"},
                {"Jwt:Issuer", "TestIssuer"},
                {"Jwt:Audience", "TestAudience"},
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _jwtService = new JwtService(configuration);
        }

        [TestMethod ]
        public void GenerateJwtToken_ShouldReturnValidToken()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "test@example.com",
                FullName = "Test User",
                Role = UserRole.Admin
            };

            // Act
            var token = _jwtService.GenerateJwtToken(user);

            // Assert
            Xunit.Assert.False(string.IsNullOrWhiteSpace(token));

            // Parse token to validate structure
            var handler = new JwtSecurityTokenHandler();
            var readable = handler.CanReadToken(token);
            Xunit.Assert.True(readable);

            var jwtToken = handler.ReadJwtToken(token);
            Xunit.Assert.Equal("test@example.com", jwtToken.Payload["email"]);
            Xunit.Assert.Equal("Test User", jwtToken.Payload["name"]);
            Xunit.Assert.Equal("Admin", jwtToken.Payload[ClaimTypes.Role]);
        }
    }
}
