using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHERRY.BUS.ViewModels.CategoriesVariants
{
    public class CategoriesVariantsVM
    {
        public Guid IDCategories { get; set; }
        public Guid IDVariants { get; set; }
        public string VariantName { get; set; } = null!;
        public decimal MinPrice { get; set; } 
        public decimal MaxPrice { get; set; }
        public string Description { get; set; }
        public List<string> ImagesURL { get; set; } = null!;
        public double Rating { get; set; }
        public DateTime CreateDate { get; set; }
        public int Status { get; set; }
    }
}
