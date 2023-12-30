using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHERRY.BUS.ViewModels.VoucherHistory
{
    public class VoucherHistoryUpdateVM
    {
        public string? ModifiedBy { get; set; }
        public Guid IDOrder { get; set; }
        public Guid IDVoucher { get; set; }

        public decimal MoneyBeforReducition { get; set; }
        public decimal MoneyAfterReducition { get; set; }
        public decimal MoneyReducition { get; set; }
        public int Status { get; set; }
    }
}
