using AutoMapper;
using CHERRY.BUS.ViewModels.Colors;
using CHERRY.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHERRY.BUS.AutoMapperConfigurations
{
    public class ColorsMap : Profile
    {
        public ColorsMap()
        {
            CreateMap<Colors, ColorsVM>().ReverseMap();
        }
    }
}
