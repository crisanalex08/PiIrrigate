using PiIrrigateServer.Enums;

namespace PiIrrigateServer.Models
{
    public class CreateScheduleRequest
    {
        public DateTime StartTime { get; set; }
        public int Duration { get; set; }
        public int Interval { get; set; }
        public DateTime EndTime { get; set; }
        public string ZoneId { get; set; }
        public string DeviceId { get; set; }
    }
}
