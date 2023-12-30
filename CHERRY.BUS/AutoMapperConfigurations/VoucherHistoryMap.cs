using AutoMapper;
using CHERRY.BUS.ViewModels.VoucherHistory;
using CHERRY.DAL.Entities;

namespace CHERRY.BUS.AutoMapperConfigurations
{
    public class VoucherHistoryMap : Profile
    {
        public VoucherHistoryMap()
        {
            CreateMap<VoucherHistory, VoucherHistoryVM>().ReverseMap();
        }
    }
}
