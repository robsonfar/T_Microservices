using AutoMapper;

using T_Microservices.Services.ProductAPI.Models;
using T_Microservices.Services.ProductAPI.Models.Dto;

namespace T_Microservices.Services.ProductAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductDto, Product>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
