using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHERRY.BUS.ViewModels
{
    public class EmailModel
    {
        public string RecipientEmail { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }

}
