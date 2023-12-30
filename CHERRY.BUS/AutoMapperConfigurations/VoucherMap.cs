using AutoMapper;
using CHERRY.BUS.ViewModels.Voucher;
using CHERRY.DAL.Entities;

namespace CHERRY.BUS.AutoMapperConfigurations
{
    public class VoucherMap : Profile
    {
        public VoucherMap()
        {
            CreateMap<Voucher, VoucherVM>().ReverseMap();
        }
    }
}
