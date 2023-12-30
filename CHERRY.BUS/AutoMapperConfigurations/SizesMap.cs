using AutoMapper;
using CHERRY.BUS.ViewModels.Sizes;
using CHERRY.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHERRY.BUS.AutoMapperConfigurations
{
    public class SizesMap : Profile
    {
        public SizesMap()
        {
            CreateMap<Sizes, SizesVM>().ReverseMap();
        }
    }
}
