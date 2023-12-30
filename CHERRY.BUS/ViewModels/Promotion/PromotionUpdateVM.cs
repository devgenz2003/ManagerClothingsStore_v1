using static CHERRY.DAL.Entities.Promotion;
using static CHERRY.DAL.Entities.Voucher;

namespace CHERRY.BUS.ViewModels.Promotion
{
    public class PromotionUpdateVM
    {
        public string? ModifiedBy { get; set; }
        public string SKU { get; set; } = null!;
        public string Content { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }

        public decimal DiscountAmount { get; set; }
        public Types Type { get; set; }
        public int Status { get; set; }
    }
}
