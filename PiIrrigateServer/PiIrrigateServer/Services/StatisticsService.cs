using PiIrrigateServer.Models;

namespace PiIrrigateServer.Services
{

    public interface IStatisticsService
    {
        public SensorReading GetStoredData();
    }
    public class StatisticsService
    {
    }
}
