using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CHERRY.DAL.Entities.Voucher;

namespace CHERRY.BUS.ViewModels.PromotionVariants
{
    public class PromotionVariantsVM
    {
        public Guid IDVariant { get; set; }
        public Guid IDPromotion { get; set; }
        public string VariantName { get; set; } = null!;
        public decimal DiscountedPrice_Promotion { get; set; } 
        public DateTime TimeRemaining { get; set; }
        public decimal RetailsPrice { get; set; } 
        public Types Types { get; set; }
        public List<string> ImagesURL { get; set; }
        public int Status { get; set; }
    }
}
