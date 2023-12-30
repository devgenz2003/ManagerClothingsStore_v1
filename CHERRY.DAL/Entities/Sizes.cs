using CHERRY.DAL.Entities.Base;

namespace CHERRY.DAL.Entities
{
    public partial class Sizes : EntityBase
    {
        public string Name { get; set; } = null!;
        public string? HexCode { get; set; }
        public virtual ICollection<Options> Options { get; set; }
    }
}
