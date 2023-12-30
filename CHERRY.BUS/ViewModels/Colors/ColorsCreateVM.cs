using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHERRY.BUS.ViewModels.Colors
{
    public class ColorsCreateVM
    {
        public string CreateBy { get; set; }
        public string Name { get; set; } = null!;
        public string? HexCode { get; set; }
        public int Status { get; set; }
    }
}
