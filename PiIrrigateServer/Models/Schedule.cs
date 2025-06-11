using PiIrrigateServer.Enums;

namespace PiIrrigateServer.Models
{
    public class Schedule
    {
        public int id { get; set; }
        public C2DMessageRequest request { get; set; }    

        public DateTime startTime { get; set; }
        
        public DateTime endTime { get; set; }
        public ScheduleStatus scheduleStatus { get; set; }
    }
}
