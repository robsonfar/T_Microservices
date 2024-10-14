using AutoMapper;

using T_Microservices.Services.ShoppingCartAPI.Models;
using T_Microservices.Services.ShoppingCartAPI.Models.Dto;

namespace T_Microservices.Services.ShoppingCartAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CartHeader, CartHeaderDto>().ReverseMap();
                config.CreateMap<CartDetails, CartDetailsDto>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
