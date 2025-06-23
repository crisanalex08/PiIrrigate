using Microsoft.EntityFrameworkCore;
using PiIrrigateServer.Database;
using PiIrrigateServer.Enums;
using PiIrrigateServer.Migrations;
using PiIrrigateServer.Models;

namespace PiIrrigateServer.Services
{
    public class ScheduleBackgroundService : BackgroundService
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly C2DMessageSender c2dSender;

        public ScheduleBackgroundService(IServiceScopeFactory scopeFactory, C2DMessageSenderManager c2DMessageSenderManager)
        {
            this.applicationDbContext = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();
            this.c2dSender = c2DMessageSenderManager.GetC2DMessageSender();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await ProcessSchedulesAsync(stoppingToken);
        }

        private async Task ProcessSchedulesAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var schedules = await applicationDbContext.Schedules.Where(s => s.StartTime <=
                DateTime.UtcNow.AddMinutes(1))
                    .ToListAsync(stoppingToken);

                foreach (var schedule in schedules)
                {
                    if (schedule.ScheduleStatus == ScheduleStatus.NotStarted)
                    {
                        try
                        {
                            schedule.ScheduleStatus = ScheduleStatus.Running;
                            await c2dSender.SendC2DMessage(schedule.ZoneId.ToString(), getStartIrrigationMethodCall(schedule.DeviceId));

                            // Update the schedule status to Running in db
                            applicationDbContext.Schedules.Update(schedule);
                        }
                        catch (Exception e)
                        {
                            schedule.ScheduleStatus = ScheduleStatus.Failed;
                        }
                    }

                    if (schedule.ScheduleStatus == ScheduleStatus.Running)
                    {
                        if( schedule.EndTime <= DateTime.UtcNow)
                        {
                            schedule.ScheduleStatus = ScheduleStatus.Completed;
                            // Update the schedule status to Completed in db
                            applicationDbContext.Schedules.Update(schedule);
                        }
                    }

                    await applicationDbContext.SaveChangesAsync(stoppingToken);
                }
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private C2DMethodCall getStartIrrigationMethodCall (string deviceId)
        {
            return new C2DMethodCall
            {
                DeviceId = deviceId,
                Method = "openRelay"
            };
        }
    }
}
