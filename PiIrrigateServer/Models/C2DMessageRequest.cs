using System.ComponentModel.DataAnnotations;

namespace PiIrrigateServer.Models
{
    public class C2DMessageRequest
    {
        [Key]
        public Guid ZoneId { get; set; }
        public C2DMethodCall methodCall { get; set; }
    }
}
