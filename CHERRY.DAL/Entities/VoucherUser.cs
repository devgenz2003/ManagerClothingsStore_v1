using CHERRY.DAL.Entities.Base;

namespace CHERRY.DAL.Entities
{
    public partial class VoucherUser : NoIDEntityBase
    {
        public Guid IDVoucher {  get; set; }
        public string IDUser { get; set; }

        public virtual User Users { get; set; }
        public virtual Voucher Voucher { get; set; }
    }
}
