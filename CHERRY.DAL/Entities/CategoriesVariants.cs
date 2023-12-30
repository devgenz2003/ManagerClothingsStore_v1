
using CHERRY.DAL.Entities.Base;

namespace CHERRY.DAL.Entities
{
    public partial class CategoriesVariants : NoIDEntityBase
    {
        public Guid IDCategories {  get; set; }
        public Guid IDVariants { get; set; }

        public virtual Variants Variants { get; set; }
        public virtual Categories Categories { get; set; }

    }
}
