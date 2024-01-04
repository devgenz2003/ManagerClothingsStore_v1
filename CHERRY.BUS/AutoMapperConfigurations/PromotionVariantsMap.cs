using AutoMapper;
using CHERRY.BUS.ViewModels.PromotionVariants;
using CHERRY.DAL.Entities;

public class PromotionVariantsMap : Profile
{
    public PromotionVariantsMap()
    {
        CreateMap<PromotionVariant, PromotionVariantsVM>()
            .ForMember(dest => dest.VariantName, opt => opt.MapFrom(src => src.Variants.VariantName))
            .ForMember(dest => dest.DiscountedPrice_Promotion, opt => opt.MapFrom(src => src.Promotion.DiscountAmount))
           // .ForMember(dest => dest.TimeRemaining, opt => opt.MapFrom(src => GetTimeRemaining(src.Promotion.StartDate, src.Promotion.EndDate)))
            .ForMember(dest => dest.ImagesURL, opt => opt.MapFrom(src => src.Variants.MediaAssets.Select(c=>c.Path)))
            .ForMember(dest => dest.Types, opt => opt.MapFrom(src => src.Promotion.Type)) 
            .ReverseMap();
    }
    private TimeSpan GetTimeRemaining(DateTime startDate, DateTime endDate)
    {
        DateTime now = DateTime.Now;
        TimeSpan remainingTime = endDate > now ? endDate - now : TimeSpan.Zero;

        return remainingTime;
    }
}
