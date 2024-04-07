using AutoMapper;
using ECommerce.Services.ShoppingCartAPI.DTOs.Cart;
using ECommerce.Services.ShoppingCartAPI.Models;

namespace ECommerce.Services.ShoppingCartAPI
{
    public class AutoMapperProfile : Profile
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config=>
            {
                config.CreateMap<CartHeader, CartHeaderDto>();
                config.CreateMap<CartDetails, CartDetailsDto>();
            });
            return mappingConfig;
        }
    }
}
