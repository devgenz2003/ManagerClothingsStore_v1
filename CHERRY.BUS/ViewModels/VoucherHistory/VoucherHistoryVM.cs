
namespace CHERRY.BUS.ViewModels.VoucherHistory
{
    public class VoucherHistoryVM
    {
        public Guid IDOrder { get; set; }
        public Guid IDVoucher { get; set; }

        public decimal MoneyBeforReducition { get; set; }
        public decimal MoneyAfterReducition { get; set; }
        public decimal MoneyReducition { get; set; }
        public int Status { get; set; }
    }
}
