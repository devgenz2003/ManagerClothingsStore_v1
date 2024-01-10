using CHERRY.DAL.Entities.Base;

namespace CHERRY.DAL.Entities
{
    public class DiscountHistory : EntityBase
    {
        public DateTime Timestamp { get; set; }
        public Guid IDVariant { get; set; }
        public decimal PreviousDiscountedPrice { get; set; }
        public decimal NewDiscountedPrice { get; set; }

        public Variants Variant { get; set; }
    }
}
