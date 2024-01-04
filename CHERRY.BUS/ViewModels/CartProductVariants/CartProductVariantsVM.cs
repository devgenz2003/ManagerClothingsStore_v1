using Microsoft.AspNetCore.Http;

namespace CHERRY.BUS.ViewModels.CartProductVariants
{
    public class CartProductVariantsVM
    {
        public Guid ID_Cart { get; set; }
        public Guid? IDOptions { get; set; }
        public string ProductName { get; set; } = null!;
        public string? SizeName { get; set; } 
        public string? ColorName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal DiscountedPrice { get; set; }
        public decimal Total_Amount { get; set; }
        public string? Notes { get; set; }
        public string Imagepaths { get; set; } 
        public int Status { get; set; }
    }
}
