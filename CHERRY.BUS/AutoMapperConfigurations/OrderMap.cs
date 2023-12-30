using AutoMapper;
using CHERRY.BUS.ViewModels.Order;
using CHERRY.DAL.Entities;

namespace CHERRY.BUS.AutoMapperConfigurations
{
    public class OrderMap : Profile
    {
        public OrderMap()
        {
            CreateMap<OrderCreateVM, Order>()
                .ForMember(dest => dest.IDUser, opt => opt.MapFrom(src => src.IDUser))
                .ForMember(dest => dest.CreateDate, opt => opt.Ignore())  // Sẽ được set bên ngoài ánh xạ
                .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => OrderStatus.Pending))
                .ForMember(dest => dest.ShipDate, opt => opt.MapFrom(src => src.ShipDate))
                .ForMember(dest => dest.ShippingAddress, opt => opt.MapFrom(src => src.ShippingAddress))
                .ForMember(dest => dest.TrackingCheck, opt => opt.MapFrom(src => src.TrackingCheck))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod))
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.PaymentStatus))
                .ForMember(dest => dest.ShippingMethod, opt => opt.MapFrom(src => src.ShippingMethod))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => 1))
                .AfterMap((src, dest) =>
                {
                    dest.CreateDate = DateTime.UtcNow;
                });

            CreateMap<Order, OrderVM>()
                .ReverseMap();

        }
    }
}
