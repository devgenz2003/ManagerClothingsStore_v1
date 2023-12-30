
namespace CHERRY.BUS.ViewModels.OrderVariant
{
    public class OrderVariantCreateVM
    {
        public string CreateBy { get; set; }
        public Guid ID { get; set; } = Guid.NewGuid();
        public Guid IDOrder { get; set; }
        public Guid IDOptions { get; set; }
        public int Quantity { get; set; } // Số lượng sản phẩm
        public decimal UnitPrice { get; set; } // Giá của một đơn vị sản phẩm
        public decimal? Discount { get; set; } // Mức giảm giá (nếu có)
                                            
        public decimal TotalAmount
        {
            get
            {
                decimal discountValue = Discount ?? 0;

                return Quantity * UnitPrice * (1 - discountValue);
            }
        }
        public bool HasPurchased { get; set; } = false;
        public bool HasReviewed { get; set; } = false;
        public int Status { get; set; }
    }
}
