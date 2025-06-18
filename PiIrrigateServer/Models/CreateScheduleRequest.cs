using PiIrrigateServer.Enums;

namespace PiIrrigateServer.Models
{
    public class CreateScheduleRequest
    {
        public C2DMessageRequest Request { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public TimeSpan Interval { get; set; }
        public bool AutoStop { get; set; }
        public DateTime EndTime { get; set; }
        public ScheduleStatus ScheduleStatus { get; set; }
    }
}
