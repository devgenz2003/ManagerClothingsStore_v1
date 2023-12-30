using CHERRY.DAL.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHERRY.DAL.Entities
{
    public partial class Material : EntityBase
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public virtual ICollection<Variants>? Variants { get; set; }
    }
}
