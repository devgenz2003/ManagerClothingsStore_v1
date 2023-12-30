using CHERRY.DAL.Entities.Base;

namespace CHERRY.DAL.Entities
{
    public partial class MediaAssets : EntityBase
    {
        public Guid? IDVariant { get; set; }
        public Guid? IDReview { get; set; }
        public string Path { get; set; }
        public string? Tags { get; set; } 
        public string? AltText { get; set; }

        public virtual Variants Variants { get; set; }
        public virtual Review Review { get; set; }
    }
}
