using CHERRY.DAL.Entities.Base;

namespace CHERRY.DAL.Entities
{
    public partial class CartProductVariants : NoIDEntityBase
    {
        public Guid? IDOptions { get; set; }
        public Guid? IDCart { get; set; }
        public int Quantity { get; set; }
        public decimal Total_Amount { get; set; }
        public string? Notes { get; set; }
        public virtual Cart Cart { get; set; }
        public virtual Options? Options { get; set; }
    }
}
