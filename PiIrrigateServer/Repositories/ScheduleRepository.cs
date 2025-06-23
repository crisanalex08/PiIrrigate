using Microsoft.EntityFrameworkCore;
using PiIrrigateServer.Database;
using PiIrrigateServer.Models;

namespace PiIrrigateServer.Repositories
{
    public interface IScheduleRepository
    {
        public Task CreateSchedule(Schedule schedule);
        
        public Task<Schedule> GetScheduleById(int id);
        public Task<IEnumerable<Schedule>> GetAllSchedules();
        public Task UpdateSchedule(Schedule schedule);
        public Task DeleteSchedule(int id);
    }
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly IServiceScopeFactory serviceScopeFactory;

        public ScheduleRepository(IServiceScopeFactory scopeFactory)
        {
            this.serviceScopeFactory = scopeFactory;
        }

        public async Task CreateSchedule(Schedule schedule)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            dbContext.Schedules.Add(schedule);
            await dbContext.SaveChangesAsync();
        }

        public async Task<Schedule> GetScheduleById(int id)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            return await dbContext.Schedules.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Schedule>> GetAllSchedules()
        {
            using var scope = serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            return await dbContext.Schedules.ToListAsync();
        }

        public async Task UpdateSchedule(Schedule schedule)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            dbContext.Schedules.Update(schedule);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteSchedule(int id)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var schedule = await dbContext.Schedules.FirstOrDefaultAsync(s => s.Id == id);
            if (schedule != null)
            {
                dbContext.Schedules.Remove(schedule);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
