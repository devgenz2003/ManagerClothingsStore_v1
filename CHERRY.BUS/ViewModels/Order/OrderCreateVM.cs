using CHERRY.BUS.ViewModels.OrderVariant;
using CHERRY.DAL.Entities;
using System.Text;

namespace CHERRY.BUS.ViewModels.Order
{
    public class OrderCreateVM
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public Guid CreateBy { get; set; }
        public int HexCode { get; set; }
        public string? VoucherCode { get; set; }
        public string? IDUser { get; set; }
        public string CustomerName { get; set; } = null!; // Tên khách hàng
        public string CustomerPhone { get; set; } = null!; // Số điện thoại khách hàng
        public string CustomerEmail { get; set; } = null!;// Địa chỉ email khách hàng
        public string ShippingAddress { get; set; }
        public string? Province { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }
        public string? ShippingAddressLine2 { get; set; }
        public DateTime ShipDate { get; set; } = DateTime.UtcNow.AddDays(3);
        public decimal TotalAmount { get; set; }
        public decimal? Cotsts { get; set; }
        public string? Notes { get; set; }
        public bool TrackingCheck { get; set; } = false; 

        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; } 
        public ShippingMethod ShippingMethod { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Processing;
        public int Status { get; set; } = 1;
        public List<OrderVariantCreateVM> OrderVariantCreateVM { get; set; }
        public int GenerateHexCode()
        {
            var now = DateTime.Now;
            var dateString = now.ToString("yyyyMMdd"); // Định dạng YYYYMMDD

            var random = new Random();
            var randomPart = random.Next(1000, 9999);

            var hexString = dateString + randomPart.ToString();

            if (int.TryParse(hexString, out int hexCode))
            {
                return hexCode;
            }
            else
            {
                randomPart = random.Next(100, 999);
                hexString = dateString.Substring(2) + randomPart.ToString(); 
                return int.Parse(hexString); 
            }
        }
    }
}
