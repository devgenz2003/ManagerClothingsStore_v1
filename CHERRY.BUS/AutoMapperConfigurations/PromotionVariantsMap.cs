using AutoMapper;
using CHERRY.BUS.ViewModels.PromotionVariants;
using CHERRY.DAL.Entities;

public class PromotionVariantsMap : Profile
{
    public PromotionVariantsMap()
    {
        CreateMap<PromotionVariant, PromotionVariantsVM>()
            .ForMember(dest => dest.VariantName, opt => opt.MapFrom(src => src.Variants.VariantName)) // Đảm bảo rằng 'Variant' là navigation property chính xác
            .ForMember(dest => dest.DiscountedPrice, opt => opt.MapFrom(src => src.Promotion.DiscountAmount)) // Và các thuộc tính khác nếu cần
            .ReverseMap();
    }
}
