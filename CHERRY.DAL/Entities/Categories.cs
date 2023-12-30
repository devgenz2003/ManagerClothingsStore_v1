using CHERRY.DAL.Entities.Base;

namespace CHERRY.DAL.Entities
{
    public partial class Categories : EntityBase
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public virtual ICollection<CategoriesVariants> CategoriesVariants { get; set; }
    }
}
