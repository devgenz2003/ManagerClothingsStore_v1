using CHERRY.DAL.Entities;

namespace CHERRY.BUS.ViewModels.Variants
{
    public class VariantsVM
    {
        public DateTime CreateDate { get; set; }
        public string VariantName { get; set; }
        public string BrandName { get; set; } = null!;
        public string MaterialName { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        public int CountReview { get; set; }
        public decimal Minprice { get; set; }
        public decimal Maxprice { get; set; }
        public int TotalOptions { get; set; }
        public int TotalQuantity { get; set; }
        //
        public Guid ID { get; set; }
        public Guid IDBrand { get; set; }
        public Guid IDMaterial { get; set; }
        public Guid IDCategory { get; set; }
        public string Description { get; set; } = null!;
        public string? SizeName { get; set; } 
        public string? ColorName { get; set; } 
        public string Style { get; set; } = null!;
        public string Origin { get; set; } = null!;
        public string SKU_v2 { get; set; } = null!;
        public bool HasPromotions { get; set; }
        public List<string> ImagePaths { get; set; } 
        public int Status { get; set; }

    }
}
