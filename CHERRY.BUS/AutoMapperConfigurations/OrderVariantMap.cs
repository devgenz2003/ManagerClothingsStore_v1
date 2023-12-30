using AutoMapper;
using CHERRY.BUS.ViewModels.OrderVariant;
using CHERRY.DAL.Entities;

namespace CHERRY.BUS.AutoMapperConfigurations
{
    public class OrderVariantMap : Profile
    {
        public OrderVariantMap()
        {
           CreateMap<OrderVariant, OrderVariantVM>()
                .ForMember(c=>c.VariantName, opt => opt.MapFrom(c=>c.Options.Variants.VariantName))
                .ForMember(c=>c.ColorName, opt=> opt.MapFrom(c=>c.Options.Color.Name))
                .ForMember(c=>c.SizeName, opt=> opt.MapFrom(c=>c.Options.Sizes.Name))
                .ReverseMap();
            CreateMap<OrderVariantVM, OrderVariantCreateVM>();
        }
    }
}
