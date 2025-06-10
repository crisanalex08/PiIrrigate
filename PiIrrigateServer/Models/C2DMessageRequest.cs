namespace PiIrrigateServer.Models
{
    public class C2DMessageRequest
    {
        public Guid ZoneId { get; set; }
        public C2DMethodCall methodCall { get; set; }
    }
}
