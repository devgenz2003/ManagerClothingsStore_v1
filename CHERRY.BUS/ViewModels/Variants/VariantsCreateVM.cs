using Microsoft.AspNetCore.Http;

namespace CHERRY.BUS.ViewModels.Variants
{
    public class VariantsCreateVM
    {
        public string? CreateBy { get; set; }
        public Guid ID { get; set; } = Guid.NewGuid();
        public Guid? IDCategory { get; set; }
        public Guid? IDBrand { get; set; }
        public Guid? IDMaterial { get; set; }
        public string VariantName { get; set; } 
        public string Description { get; set; }
        public string MaterialName { get; set; } = null!;
        public string BrandName { get; set; }
        public string CategoryName { get; set; }
        public string Style { get; set; } 
        public string Origin { get; set; }
        public string SKU_v2 { get; set; } 
        public List<IFormFile> ImagePaths { get; set; } 
        public int Status { get; set; }
    }
}
