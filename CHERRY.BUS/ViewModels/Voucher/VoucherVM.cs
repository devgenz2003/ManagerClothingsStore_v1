using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CHERRY.DAL.Entities.Voucher;

namespace CHERRY.BUS.ViewModels.Voucher
{
    public class VoucherVM
    {
        public Guid ID { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Quantity { get; set; }
        public Types Type { get; set; }
        public int MinimumAmount { get; set; }
        public decimal ReducedValue { get; set; }
        public bool IsActive { get; set; }
        public string Key { get; set; } = null!;

        public int Status { get; set; }
    }
}
