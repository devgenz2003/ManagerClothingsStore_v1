using CHERRY.DAL.Entities.Base;

namespace CHERRY.DAL.Entities
{
    public partial class Variants : EntityBase
    {
        public Guid IDBrand { get; set; }
        public Guid IDMaterial { get; set; }
        public string VariantName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Style { get; set; } = null!;
        public string Origin { get; set; } = null!;
        public string SKU_v2 { get; set; } = null!; // Mã SKU của biến thể 
        public virtual ICollection<CategoriesVariants> CategoriesVariants { get; set; } = null!;
        public virtual Brand Brand { get; set; } = null!;
        public virtual Material Material { get; set; } = null!;
        public virtual ICollection<PromotionVariant>? PromotionVariants { get; set; }
        public virtual ICollection<Options>? Options { get; set; } 
        public virtual ICollection<MediaAssets> MediaAssets { get; set; } = null!;
        public virtual ICollection<Review>? Reviews { get; set; }

    }
}
