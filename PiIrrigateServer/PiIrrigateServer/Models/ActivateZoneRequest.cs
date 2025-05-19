namespace PiIrrigateServer.Models
{
    public class ActivateZoneRequest
    {
        public string ActivationCode { get; set; }
        public string ZoneName { get; set; }
        public Guid UserId { get; set; }
        public int RefreshInterval { get; set; } = 5; // Default refresh interval in minutes
    }
}
