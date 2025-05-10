using PiIrrigateServer.Enums;

namespace PiIrrigateServer.Models
{
    public class User
    {
        public Guid Id { get; set; } // Unique identifier
        public string FullName { get; set; }
        public string Email { get; set; } // Unique email for authentication
        public string PasswordHash { get; set; } // Store hashed password
        public UserRole Role { get; set; } // e.g., "Admin", "User"
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation Properties
        public ICollection<Zone> Zones { get; set; } = new List<Zone>();
    }
}
