
using Microsoft.AspNetCore.Http;

namespace CHERRY.BUS.ViewModels.Variants
{
    public class VariantsUpdateVM
    {
        public string? ModifieBy { get; set; }
        public Guid IDBrand { get; set; }
        public Guid IDMaterial { get; set; }
        public string VariantName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Style { get; set; } = null!;
        public string Origin { get; set; } = null!;
        public string SKU_v2 { get; set; } = null!; 
        public List<IFormFile> ImagePaths { get; set; } = null!;

        public int Status { get; set; }
    }
}
