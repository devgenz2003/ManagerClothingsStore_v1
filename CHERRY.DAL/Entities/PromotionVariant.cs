using CHERRY.DAL.Entities.Base;
namespace CHERRY.DAL.Entities
{
    public partial class PromotionVariant : NoIDEntityBase
    {
        public Guid IDVariant { get; set; }
        public Guid IDPromotion {  get; set; }

        public virtual Promotion Promotion { get; set; }
        public virtual Variants Variants { get; set; }
    }
}
