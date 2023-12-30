namespace CHERRY.BUS.ViewModels.OrderVariant
{
    public class OrderVariantlUpdateVM
    {
        public Guid? ModifieBy { get; set; }
        public Guid IDOrder { get; set; }
        public Guid IDOptions { get; set; }
        public int Quantity { get; set; } // Số lượng sản phẩm
        public decimal UnitPrice { get; set; } // Giá của một đơn vị sản phẩm
        public decimal Discount { get; set; } // Mức giảm giá (nếu có)
        public decimal TotalAmount { get; set; }
        public bool HasPurchased { get; set; } 
        public bool HasReviewed { get; set; }
        public int Status { get; set; }
    }
}
