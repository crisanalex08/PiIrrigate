namespace PiIrrigateServer.Models
{
    public class RegisterDeviceRequest
    {
        public string Mac { get; set; } // Unique identifier for the device
        public string Name { get; set; } // Name of the device
        public string Location { get; set; } // Location of the device
        public string Owner { get; set; } // Owner of the device
        public string Description { get; set; } // Description of the device
    }
}