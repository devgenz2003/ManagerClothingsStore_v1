using CHERRY.DAL.Entities.Base;

namespace CHERRY.DAL.Entities
{
    public partial class Colors : EntityBase
    {
        public string Name { get; set; } = null!;
        public string? HexCode { get; set; }
        public virtual ICollection<Options> Options { get; set; }
    }
}
