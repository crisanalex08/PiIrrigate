using PiIrrigateServer.Enums;

namespace PiIrrigateServer.Models
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }  // e.g., "Admin", "User"
        public DateTime CreatedAt { get; set; }
    }

}
