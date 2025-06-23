using PiIrrigateServer.Enums;

namespace PiIrrigateServer.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public Guid ZoneId { get; set; }
        public string DeviceId { get; set; }
        public DateTime StartTime { get; set; }
        public int Duration { get; set; }
        public int Interval { get; set; }
        public DateTime EndTime { get; set; }
        public bool AutoStop { get; set; }
        public ScheduleStatus ScheduleStatus { get; set; }
    }
}
