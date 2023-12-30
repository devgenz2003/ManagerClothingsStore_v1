using CHERRY.DAL.Entities.Base;

namespace CHERRY.DAL.Entities
{
    public partial class VoucherHistory : NoIDEntityBase
    {
        public Guid IDOrder { get; set; }
        public Guid IDVoucher { get; set; }

        public decimal MoneyBeforReducition { get; set; }
        public decimal MoneyAfterReducition { get; set; }
        public decimal MoneyReducition { get; set; }

        public virtual Order Order { get; set; }
        public virtual Voucher Voucher { get; set; }
    }
}
