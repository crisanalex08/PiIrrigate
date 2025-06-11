using Microsoft.AspNetCore.Mvc;
using PiIrrigateServer.Models;
using PiIrrigateServer.Repositories;

namespace PiIrrigateServer.Controllers
{
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleRepository scheduleRepository;

        public ScheduleController(IScheduleRepository scheduleRepository)
        {
            this.scheduleRepository = scheduleRepository;
            // Initialize any required services or dependencies here
        }

        [HttpPost("api/schedule/create")]
        public IActionResult CreateSchedule([FromBody] CreateScheduleRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid schedule request.");
            }

            if (request.StartTime >= request.EndTime)
            {
                return BadRequest("Start time must be before end time.");
            }

            if (request.StartTime <= DateTime.UtcNow || request.EndTime <= DateTime.UtcNow)
            {
                return BadRequest("Start and end times must be in the future.");
            }

            scheduleRepository.CreateSchedule(new Schedule
            {
                request = request.Request,
                startTime = request.StartTime,
                endTime = request.EndTime,
                scheduleStatus = Enums.ScheduleStatus.NotStarted
            });

            return Ok("Schedule created successfully.");
        }

        [HttpGet("api/schedule/{id}")]
        public async Task<IActionResult> GetScheduleById(int id)
        {
            var schedule = await scheduleRepository.GetScheduleById(id);
            if (schedule == null)
            {
                return NotFound("Schedule not found.");
            }

            return Ok(schedule);
        }

        [HttpGet("api/schedule")]
        public async Task<IActionResult> GetAllSchedules()
        {
            var schedules = await scheduleRepository.GetAllSchedules();
            return Ok(schedules);
        }

        [HttpPut("api/schedule/update")]
        public async Task<IActionResult> UpdateSchedule([FromBody] Schedule schedule)
        {
            if (schedule == null || schedule.id <= 0)
            {
                return BadRequest("Invalid schedule data.");
            }

            var existingSchedule = await scheduleRepository.GetScheduleById(schedule.id);
            if (existingSchedule == null)
            {
                return NotFound("Schedule not found.");
            }

            // Update the existing schedule with new values
            existingSchedule.request = schedule.request;
            existingSchedule.startTime = schedule.startTime;
            existingSchedule.endTime = schedule.endTime;
            existingSchedule.scheduleStatus = schedule.scheduleStatus;

            await scheduleRepository.UpdateSchedule(existingSchedule);
            return Ok("Schedule updated successfully.");
        }
    }
}
