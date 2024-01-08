using AutoMapper;
using CHERRY.BUS.ViewModels.VoucherUser;
using CHERRY.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHERRY.BUS.AutoMapperConfigurations
{
    public class VoucherUserMap : Profile
    {
        public VoucherUserMap()
        {
            CreateMap<VoucherUser, VoucherUserVM>().ReverseMap();
        }
    }
}
