using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CHERRY.DAL.Entities.Base
{
    public class EntityBase
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        [BindProperty, DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public string CreateBy { get; set; }
        [BindProperty, DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? ModifieDate { get; set; }
        public string? ModifieBy { get; set; }
        [BindProperty, DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DeleteDate { get; set; } 
        public string? DeleteBy { get; set; }
        public int Status { get; set; } // 0 = Delete
    }
}
