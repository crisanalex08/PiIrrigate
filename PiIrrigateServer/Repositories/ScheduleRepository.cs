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

        public Task CreateSchedule(Schedule schedule)
        {
            throw new NotImplementedException();
        }

        public Task DeleteSchedule(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Schedule>> GetAllSchedules()
        {
            throw new NotImplementedException();
        }

        public Task<Schedule> GetScheduleById(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateSchedule(Schedule schedule)
        {
            throw new NotImplementedException();
        }
    }
}
