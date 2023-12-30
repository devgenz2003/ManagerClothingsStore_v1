using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CHERRY.DAL.Entities.Voucher;

namespace CHERRY.BUS.ViewModels.Voucher
{
    public class VoucherCreateVM
    {
        public string CreateBy { get; set; }
        public Guid ID { get; set; } = Guid.NewGuid();
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Quantity { get; set; }
        public Types Type { get; set; } 
        public int MinimumAmount { get; set; }
        public int ReducedValue { get; set; }
        public bool IsActive { get; set; }
        public string Key { get; set; } = null!;
        public List<string> SelectedUser { get; set; } = new List<string>();

        public int Status { get; set; }
    }
}
