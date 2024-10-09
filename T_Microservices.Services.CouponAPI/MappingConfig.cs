using AutoMapper;

using T_Microservices.Services.CouponAPI.Models;
using T_Microservices.Services.CouponAPI.Models.Dto;

namespace T_Microservices.Services.CouponAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CouponDto, Coupon>();
                config.CreateMap<Coupon, CouponDto>();
            });

            return mappingConfig;
        }
    }
}
