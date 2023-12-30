using CHERRY.DAL.Entities.Base;

namespace CHERRY.DAL.Entities
{
    public partial class Brand : EntityBase
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public virtual ICollection<Variants>? Variants { get; set; }
    }
}
