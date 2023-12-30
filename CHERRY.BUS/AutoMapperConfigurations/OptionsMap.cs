using AutoMapper;
using CHERRY.BUS.ViewModels.Options;
using CHERRY.DAL.Entities;

namespace CHERRY.BUS.AutoMapperConfigurations
{
    public class OptionsMap : Profile
    {
        public OptionsMap()
        {
            CreateMap<Options, OptionsVM>()
                .ForMember(dest => dest.ColorName, opt => opt.MapFrom(erc => erc.Color.Name))
                .ForMember(dest => dest.SizeName, opt => opt.MapFrom(erc => erc.Sizes.Name))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(erc => erc.Variants.VariantName))
                .ReverseMap();
        }
    }
}
