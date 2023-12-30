using CHERRY.DAL.Entities.Base;
using System.Drawing;

namespace CHERRY.DAL.Entities
{
    public partial class Options : EntityBase
    {
        public decimal CostPrice { get; set; }
        public decimal RetailPrice { get; set; }
        public decimal? DiscountedPrice { get; set; }
        public int StockQuantity { get; set; }
        public Guid? IDVariant { get; set; }
        public Guid? IDColor { get; set; }
        public Guid? IDSizes { get; set; }
       
        public string ImageURL { get; set; } = null!;
        public virtual Colors? Color { get; set; }
        public virtual Sizes? Sizes { get; set; }
        public virtual Variants? Variants { get; set; }

        public virtual ICollection<OrderVariant>? OrderVariant { get; set; } 
        public virtual ICollection<CartProductVariants>? CartProductVariants { get; set; }
    }
}
