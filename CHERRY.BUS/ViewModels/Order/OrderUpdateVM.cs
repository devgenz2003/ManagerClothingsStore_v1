using CHERRY.DAL.Entities;
namespace CHERRY.BUS.ViewModels.Order
{
    public class OrderUpdateVM
    {
        public Guid? ModifieBy { get; set; }
        public int HexCode { get; set; }
        public string? VoucherCode { get; set; }

        public string? IDUser { get; set; }
        public string CustomerName { get; set; } = null!; // Tên khách hàng
        public string CustomerPhone { get; set; } = null!; // Số điện thoại khách hàng
        public string CustomerEmail { get; set; } = null!;// Địa chỉ email khách hàng
        public string ShippingAddress { get; set; } = null!; 
        public string? ShippingAddressLine2 { get; set; }
        public DateTime ShipDate { get; set; } = DateTime.UtcNow.AddDays(3);
        public decimal TotalAmount { get; set; }
        public decimal? Cotsts { get; set; }
        public string? Notes { get; set; }
        public bool TrackingCheck { get; set; } = false; //người dùng xáC NHẬN đơn hàng => sang hoá đơn

        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; } 
        public ShippingMethod ShippingMethod { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Processing;
        public int Status { get; set; } = 1;
    }
}
