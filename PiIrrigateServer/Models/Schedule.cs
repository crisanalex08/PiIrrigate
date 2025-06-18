using PiIrrigateServer.Enums;

namespace PiIrrigateServer.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public C2DMessageRequest Request { get; set; }    
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public TimeSpan Interval { get; set; }
        public DateTime EndTime { get; set; }
        public bool AutoStop { get; set; }
        public ScheduleStatus ScheduleStatus { get; set; }
    }
}
