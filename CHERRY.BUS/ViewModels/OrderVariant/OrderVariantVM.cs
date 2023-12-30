namespace CHERRY.BUS.ViewModels.OrderVariant
{
    public class OrderVariantVM
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public Guid IDOrder { get; set; }
        public Guid IDOptions { get; set; }
        public string VariantName { get; set; } = null!;
        public string? SizeName { get; set; }
        public string? ColorName { get; set; }
        public int Quantity { get; set; } // Số lượng sản phẩm
        public decimal UnitPrice { get; set; } // Giá của một đơn vị sản phẩm
        public decimal? Discount { get; set; } // Mức giảm giá (nếu có)
        public decimal TotalAmount { get; set; }
        public bool HasPurchased { get; set; }
        public bool HasReviewed { get; set; }
        public int Status { get; set; }
    }
}
