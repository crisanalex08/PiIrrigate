using AutoMapper;
using PiIrrigateServer.Entities;
using PiIrrigateServer.Models;

namespace PiIrrigateServer.Profiles
{
    public class DataProfile : Profile
    {
        public DataProfile()
        {
            CreateMap<DataEntity, DataModel>()
                .ForMember(dest => dest.IrrigationZoneId, opt => opt.MapFrom(src => src.IrrigationZoneId))
                .ForMember(dest => dest.SensorId, opt => opt.MapFrom(src => src.SensorId))
                .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => src.Timestamp))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value));
        }
    }

    public static class DataMapperExtensions
    {
        public static async Task<List<DataModel>> MapToDataModelListAsync(
            this IMapper mapper,
            Task<List<DataEntity>> entityListTask)
        {
            var entities = await entityListTask;
            return mapper.Map<List<DataModel>>(entities);
        }
    }
}
