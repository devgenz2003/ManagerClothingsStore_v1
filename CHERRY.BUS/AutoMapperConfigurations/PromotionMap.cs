using AutoMapper;
using CHERRY.BUS.ViewModels.Promotion;
using CHERRY.DAL.Entities;
using System.Linq;

namespace CHERRY.BUS.AutoMapperConfigurations
{
    public class PromotionMap : Profile
    {
        public PromotionMap()
        {
            CreateMap<Promotion, PromotionVM>()
 .ForMember(dest => dest.IDVariant, opt => opt.MapFrom(src => src.PromotionVariants.Select(c=>c.IDVariant).ToList()))
                .ReverseMap();
        }
    }
}
