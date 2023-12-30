
using AutoMapper;
using CHERRY.BUS.ViewModels.Material;
using CHERRY.DAL.Entities;

namespace CHERRY.BUS.AutoMapperConfigurations
{
    public class MaterialMap : Profile
    {
        public MaterialMap()
        {
            CreateMap<Material, MaterialVM>().ReverseMap();
        }
    }
}
