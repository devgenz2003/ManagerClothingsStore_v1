using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CHERRY.DAL.Entities.Promotion;
using static CHERRY.DAL.Entities.Voucher;

namespace CHERRY.BUS.ViewModels.Promotion
{
    public class PromotionCreateVM
    {
        public string CreateBy { get; set; } = null!;
        public Guid ID { get; set; }
        public string SKU { get; set; } = null!;
        public string Content { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }

        public decimal DiscountAmount { get; set; }
        public Types Type { get; set; }
        public int Status { get; set; } = 1;
        public List<Guid> SelectedVariantIds { get; set; } 
    }
}
