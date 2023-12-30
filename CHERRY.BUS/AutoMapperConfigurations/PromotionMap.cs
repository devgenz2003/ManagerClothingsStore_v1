using AutoMapper;
using CHERRY.BUS.ViewModels.Promotion;
using CHERRY.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHERRY.BUS.AutoMapperConfigurations
{
    public class PromotionMap : Profile
    {
        public PromotionMap()
        {
            CreateMap<Promotion, PromotionVM>().ReverseMap();
        }
    }
}
