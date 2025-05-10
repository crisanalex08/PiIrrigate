namespace PiIrrigateServer.Models
{
    public class Zone
    {
        public Guid ZoneId { get; set; } // Unique identifier for the zone
        public string? Name { get; set; } // Name of the zone
        public string? ConnectionString { get; set; } // ConnectionString fro IotHub Device
        public ICollection<Device> Devices { get; set; } = new List<Device>(); // Array of devices in the zone
        public Guid? UserId { get; set; }
        public User User { get; set; } = null!; // Navigation property to the User entity
    }
}
