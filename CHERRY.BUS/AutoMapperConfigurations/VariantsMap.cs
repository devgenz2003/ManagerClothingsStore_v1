using AutoMapper;
using CHERRY.BUS.ViewModels.Options;
using CHERRY.BUS.ViewModels.Variants;
using CHERRY.DAL.Entities;

namespace CHERRY.BUS.AutoMapperConfigurations
{
    public class VariantsMap : Profile
    {
        public VariantsMap()
        {
            CreateMap<Variants, VariantsVM>()
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(erc => erc.Brand.Name))
                .ForMember(dest => dest.MaterialName, opt => opt.MapFrom(erc => erc.Material.Name))
                .ForMember(dest => dest.Minprice, opt => opt.MapFrom(src =>
                        src.Options != null && src.Options.Any() ? src.Options.Min(opt => opt.RetailPrice) : 0))
                .ForMember(dest => dest.Maxprice, opt => opt.MapFrom(src =>
                        src.Options != null && src.Options.Any() ? src.Options.Max(opt => opt.RetailPrice) : 0))
                .ForMember(dest => dest.TotalOptions, opt => opt.MapFrom(src =>
                        src.Options != null ? src.Options.Count() : 0))
                .ForMember(dest => dest.TotalQuantity, opt => opt.MapFrom(src =>
                        src.Options != null ? src.Options.Sum(opt => opt.StockQuantity) : 0))
                .ForMember(dest => dest.ImagePaths, opt => opt.MapFrom(src => src.MediaAssets.Select(c => c.Path)))
                 .ForMember(dest => dest.SizeName, opt => opt.MapFrom(src => src.Options
                                                                            .Where(o => o.Sizes != null)
                                                                            .Select(o => o.Sizes.Name)
                                                                            .Distinct()
                                                                            .FirstOrDefault()))
                .ForMember(dest => dest.ColorName, opt => opt.MapFrom(src => src.Options
                                                                             .Where(o => o.Color != null)
                                                                             .Select(o => o.Color.Name)
                                                                             .Distinct()
                                                                             .FirstOrDefault()))
                .ForMember(dest => dest.HasPromotions, opt => opt.MapFrom(src => src.PromotionVariants.Any(pv =>
                        pv.Promotion.IsActive &&
                        pv.Promotion.StartDate <= DateTime.Now &&
                        pv.Promotion.EndDate >= DateTime.Now))).ReverseMap();
        }
    }
}
