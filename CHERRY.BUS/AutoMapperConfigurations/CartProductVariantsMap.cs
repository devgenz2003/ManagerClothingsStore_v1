using AutoMapper;
using CHERRY.BUS.ViewModels.CartProductVariants;
using CHERRY.DAL.Entities;

namespace CHERRY.BUS.AutoMapperConfigurations
{
    public class CartProductVariantsMap : Profile
    {
        public CartProductVariantsMap()
        {
            CreateMap<CartProductVariants, CartProductVariantsVM>()
                .ReverseMap();
        }
    }
}
