using AutoMapper;
using CHERRY.BUS.ViewModels.CategoriesVariants;
using CHERRY.DAL.Entities;

namespace CHERRY.BUS.AutoMapperConfigurations
{
    public class CategoriesVariantsMap : Profile
    {
        public CategoriesVariantsMap()
        {
            CreateMap<CategoriesVariants, CategoriesVariantsVM>()
                .ForMember(dest => dest.VariantName, opt => opt.MapFrom(c => c.Variants.VariantName))
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(c => c.Variants.CreateDate))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(c => c.Variants.Description))
                .ForMember(dest => dest.MinPrice, opt => opt.MapFrom(src =>
                        src.Variants.Options.Select(c => c.RetailPrice).Min()))
                .ForMember(dest => dest.MaxPrice, opt => opt.MapFrom(src =>
                        src.Variants.Options.Select(c => c.RetailPrice).Max()))
                .ForMember(dest => dest.ImagesURL, opt => opt.MapFrom(src => src.Variants.MediaAssets.Select(c => c.Path)))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(c => CalculateAverageRating(c.Variants.Reviews)))
                .ReverseMap();
        }
        private static double CalculateAverageRating(ICollection<Review> reviews)
        {
            List<Review> reviewsList = reviews.ToList();
            if (reviewsList != null && reviewsList.Any())
            {
                double totalRating = reviewsList.Select(review => review.Rating).Sum();
                return totalRating / reviewsList.Count;
            }
            return 0; // Trường hợp không có reviews
        }
    }
}
