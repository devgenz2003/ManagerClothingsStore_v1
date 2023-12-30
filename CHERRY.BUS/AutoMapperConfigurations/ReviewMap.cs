using AutoMapper;
using CHERRY.BUS.ViewModels.Review;
using CHERRY.DAL.Entities;

namespace CHERRY.BUS.AutoMapperConfigurations
{
    public class ReviewMap : Profile
    {
        public ReviewMap()
        {
            CreateMap<Review, ReviewVM>()
                .ForMember(dest => dest.ImagePaths, opt => opt.MapFrom(erc => erc.MediaAssets.Select(c => c.Path)))
                .ForMember(dest => dest.SizeName,
                    opt => opt.MapFrom(erc =>
                    erc.OrderVariant.Options.Sizes.Name))
                .ForMember(dest => dest.ColorName,
                    opt => opt.MapFrom(erc =>
                    erc.OrderVariant.Options.Color.Name))
                                .ReverseMap();
        }
    }
}
