using AutoMapper;
using CHERRY.BUS.ViewModels.Brand;
using CHERRY.DAL.Entities;
namespace CHERRY.BUS.AutoMapperConfigurations
{
    public class BrandMap : Profile
    {
        public BrandMap()
        {
            CreateMap<Brand,BrandVM>().ReverseMap();
        }
    }
}
