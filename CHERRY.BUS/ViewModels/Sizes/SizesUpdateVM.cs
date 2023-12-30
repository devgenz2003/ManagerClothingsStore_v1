using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHERRY.BUS.ViewModels.Sizes
{
    public class SizesUpdateVM
    {
        public string ModifiedBy { get; set; }
        public string Name { get; set; } = null!;
        public string? HexCode { get; set; }
        public int Status { get; set; }
    }
}
