using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PiIrrigateServer.Database;
using PiIrrigateServer.Models;
using PiIrrigateServer.SignalR;

namespace PiIrrigateServer.Repositories
{
    public interface IDataRepository
    {
        public Task StoreData(SensorReading sensorReading);
        public Task<SensorReading> GetLastAsync();
        public Task<IEnumerable<SensorReading>> GetTimedZoneData(DateTime from, DateTime to, Guid zoneId);
        public Task<IEnumerable<SensorReading>> GetTimedDeviceData(DateTime from, DateTime to, Guid zoneId, string deviceId);
        public Task<IEnumerable<SensorReading>> GetAllZoneData(Guid zoneId);
        public Task<IEnumerable<SensorReading>> GetAllDeviceData(Guid zoneId, string DeviceId);
        public Task DeleteAll();
    }
    public class DataRepository : IDataRepository
    {
        private readonly IHubContext<LiveDataHub> hubContext;
        private readonly ApplicationDbContext dbContext;

        public DataRepository(IHubContext<LiveDataHub> hubContext, IDbContextFactory<ApplicationDbContext> dbContextFactory)
        {
            this.hubContext = hubContext;
            dbContext = dbContextFactory.CreateDbContext();
        }

        public async Task DeleteAll()
        {
            dbContext.SensorReadings.RemoveRange(dbContext.SensorReadings.ToList());
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<SensorReading>> GetAllDeviceData(Guid zoneId, string deviceId)
        {
            return await dbContext.SensorReadings
                .Where(sr => sr.ZoneId == zoneId && sr.Mac == deviceId)
                .ToListAsync();
        }

        public async Task<IEnumerable<SensorReading>> GetAllZoneData(Guid zoneId)
        {
            return await dbContext.SensorReadings
                .Where(sr => sr.ZoneId == zoneId)
                .ToListAsync();
        }

        public async Task<SensorReading> GetLastAsync()
        {
            return await dbContext.SensorReadings.OrderBy(s => s.Timestamp).LastOrDefaultAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<SensorReading>> GetTimedDeviceData(DateTime from, DateTime to, Guid zoneId, string deviceId)
        {
            return await dbContext.SensorReadings
                .Where(sr => sr.ZoneId == zoneId && sr.Timestamp >= from && sr.Timestamp <= to && sr.Mac == deviceId)
                .OrderByDescending(sr => sr.Timestamp)
                .ToListAsync();
        }

        public async Task<IEnumerable<SensorReading>> GetTimedZoneData(DateTime from, DateTime to, Guid zoneId)
        {
            return await dbContext.SensorReadings
                .Where(sr => sr.ZoneId == zoneId && sr.Timestamp >= from && sr.Timestamp <= to)
                .OrderByDescending(sr => sr.Timestamp)
                .ToListAsync();
        }

        public async Task StoreData(SensorReading sensorReading)
        {
            await CheckStorage(); // Check if we need to delete old data
            dbContext.SensorReadings.Add(sensorReading);
            await dbContext.SaveChangesAsync();
        }

        private Task CheckStorage()
        {
            var count = dbContext.SensorReadings.Count();

            if (count > 1000)
            {
                var oldestData = dbContext.SensorReadings.OrderBy(sr => sr.Timestamp).Take(count - 1000); // Get the oldest data to delete
                dbContext.SensorReadings.RemoveRange(oldestData);
                return dbContext.SaveChangesAsync();
            }
            else
            {
                return Task.CompletedTask; // No need to delete anything
            }
        }
    }
}
