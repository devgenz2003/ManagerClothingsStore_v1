using AutoMapper;
using CHERRY.BUS.ViewModels.Cart;
using CHERRY.DAL.Entities;

namespace CHERRY.BUS.AutoMapperConfigurations
{
    public class CartMap : Profile
    {
        public CartMap()
        {
            CreateMap<Cart, CartVM>()
                .ReverseMap();
        }
    }
}
