using CHERRY.DAL.Entities.Base;
namespace CHERRY.DAL.Entities
{
    public partial class Review : EntityBase
    {
        public string IDUser { get; set; } = null!;
        public Guid IDVariant { get; set; } 
        public Guid? IDOrderVariant { get; set; }
        public string Content { get; set; } = null!;
        public int Rating { get; set; }
        public bool IsPurchased { get; set; }
        public virtual User User { get; set; }
        public virtual Variants Variants { get; set; }
        public virtual OrderVariant? OrderVariant { get; set; }
        public ICollection<MediaAssets> MediaAssets { get; set; }
    }
}
