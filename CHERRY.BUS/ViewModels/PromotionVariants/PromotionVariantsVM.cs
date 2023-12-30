using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHERRY.BUS.ViewModels.PromotionVariants
{
    public class PromotionVariantsVM
    {
        public Guid IDVariant { get; set; }
        public Guid IDPromotion { get; set; }
        public string VariantName { get; set; } = null!;
        public decimal DiscountedPrice { get; set; } 
        public int Status { get; set; }
    }
}
