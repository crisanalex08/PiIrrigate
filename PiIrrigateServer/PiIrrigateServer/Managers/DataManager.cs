using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PiIrrigateServer.Database;
using PiIrrigateServer.Models;
using PiIrrigateServer.SignalR;

namespace PiIrrigateServer.Managers
{
    public interface IDataManager
    {
        public Task HandleDataMessage(SensorReading sensorReading);
        public Task<IEnumerable<SensorReading>> GetTimedZoneData (DateTime from, DateTime to, Guid zoneId);
        public Task<IEnumerable<SensorReading>> GetTimedDeviceData (DateTime from, DateTime to, Guid zoneId, string deviceId);
        public Task<IEnumerable<SensorReading>> GetAllZoneData (Guid zoneId);
        public Task<IEnumerable<SensorReading>> GetAllDeviceData (Guid zoneId, string DeviceId);
    }
    public class DataManager : IDataManager
    {
        private readonly IHubContext<LiveDataHub> hubContext;
        private readonly ApplicationDbContext dbContext;

        public DataManager(IHubContext<LiveDataHub> hubContext, IDbContextFactory<ApplicationDbContext> dbContextFactory)
        {
            this.hubContext = hubContext;
            this.dbContext = dbContextFactory.CreateDbContext();
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

        public async Task HandleDataMessage(SensorReading sensorReading)
        {
            await SendLiveData(sensorReading);
            await StoreData(sensorReading);
        }

        public async Task SendLiveData(SensorReading sensorReading)
        {
            await hubContext.Clients.Group(sensorReading.ZoneId.ToString()).SendAsync("ReceiveLiveData", sensorReading);
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
