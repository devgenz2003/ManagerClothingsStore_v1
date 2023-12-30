using Microsoft.AspNetCore.Http;

namespace CHERRY.BUS.ViewModels.Options
{
    public class OptionsCreateVM
    {
        public string CreateBy { get; set; }
        public Guid IDVariant { get; set; }
        public Guid? IDColor { get; set; }
        public Guid? IDSizes { get; set; }
        public decimal CostPrice { get; set; }
        public decimal RetailPrice { get; set; }
        public decimal? DiscountedPrice { get; set; }
        public int StockQuantity { get; set; } // Số lượng tồn kho

        public string ColorName { get; set; } 
        public string SizesName { get; set; } 
        public IFormFile ImagePaths { get; set; } = null!;
        public int Status { get; set; }
    }
}
