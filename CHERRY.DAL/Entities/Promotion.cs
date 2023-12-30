using CHERRY.DAL.Entities.Base;
using static CHERRY.DAL.Entities.Voucher;

namespace CHERRY.DAL.Entities
{
    public partial class Promotion : EntityBase
    {
        public string SKU { get; set; } // Mã sản phẩm, nếu áp dụng
        public string Content { get; set; } // Nội dung của khuyến mãi, có thể bao gồm chi tiết như hình ảnh, text, v.v.
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal DiscountAmount { get; set; }
        public bool IsActive { get; set; }
        public Types Type { get; set; } 
        
        public virtual ICollection<PromotionVariant> PromotionVariants { get; set; }
    }
}
