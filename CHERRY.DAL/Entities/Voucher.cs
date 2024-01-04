using CHERRY.DAL.Entities.Base;

namespace CHERRY.DAL.Entities
{
    public partial class Voucher : EntityBase
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Quantity { get; set; }
        public Types Type { get; set; } 
        public int MinimumAmount { get; set; }
        public decimal ReducedValue { get; set; }
        public string Key { get; set; } = null!;
        public bool IsActive { get; set; }
        public virtual ICollection<VoucherUser> VoucherUser { get; set; }
        public virtual ICollection<VoucherHistory> VoucherHistory { get; set; }

        public enum Types
        {
            Percent,
            Cash,
            Unknown
        }
    }
}
