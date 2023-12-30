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
                .ReverseMap();
        }
    }
}
