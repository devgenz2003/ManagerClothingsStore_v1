using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHERRY.BUS.ViewModels.CategoriesVariants
{
    public class CategoriesVariantsUpdateVM
    {
        public string? ModifiedBy { get; set; }
        public Guid IDCategories { get; set; }
        public Guid IDVariants { get; set; }
        public int Status { get; set; }
    }
}
