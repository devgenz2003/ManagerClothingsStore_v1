using CHERRY.DAL.Entities.Base;

namespace CHERRY.DAL.Entities
{
    public partial class OrderVariant : EntityBase
    {
        public Guid IDOrder {  get; set; }
        public Guid IDOptions {  get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal? Discount { get; set; }
        public bool HasPurchased { get; set; }
        public bool HasReviewed { get; set; }
        public virtual Options Options { get; set; }
        public virtual Order Order { get; set; } = null!; 
        public virtual ICollection<Review> Review { get; set; }

    }
}
