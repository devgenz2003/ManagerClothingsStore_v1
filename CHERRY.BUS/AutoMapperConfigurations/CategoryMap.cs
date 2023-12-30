
using AutoMapper;
using CHERRY.BUS.ViewModels.Category;
using CHERRY.DAL.Entities;

namespace CHERRY.BUS.AutoMapperConfigurations
{
    public class CategoryMap : Profile
    {
        public CategoryMap()
        {
            CreateMap<Categories, CategoryVM>().ReverseMap();
        }
    }
}
