using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PiIrrigateServer.Database;
using PiIrrigateServer.Entities;
using PiIrrigateServer.Models;
using PiIrrigateServer.Profiles;

namespace PiIrrigateServer.Services
{
    public interface IDataService
    {
        public Task <List<DataModel>> GetAllStoredData();
        public Task AddData(DataModel dataModel);
    }
    public class DataService : IDataService
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly ILogger<IDataService> logger;
        private readonly IMapper mapper;

        public DataService(IServiceScopeFactory serviceScopeFactory,
            ILogger<IDataService> logger,
            IMapper mapper)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task AddData(DataModel dataModel)
        {
            var scope = this.serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            DataEntity dataEntity = new()
            {
                IrrigationZoneId = dataModel.IrrigationZoneId,
                SensorId = dataModel.SensorId,
                Timestamp = dataModel.Timestamp,
                Value = dataModel.Value
            };

            dbContext.DataEntities.Add(dataEntity);
            await dbContext.SaveChangesAsync();
        }

        public Task<List<DataModel>> GetAllStoredData()
        {
            var scope = this.serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            return mapper.MapToDataModelListAsync(dbContext.DataEntities.ToListAsync());
        }
    }
}
