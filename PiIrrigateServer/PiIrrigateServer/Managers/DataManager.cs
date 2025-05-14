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
