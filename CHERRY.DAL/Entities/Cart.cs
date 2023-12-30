using CHERRY.DAL.Entities.Base;
namespace CHERRY.DAL.Entities
{
    public partial class Cart : EntityBase
    {
        public string? ID_User { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual ICollection<CartProductVariants> CartProductVariants { get; set; }
    }
}
