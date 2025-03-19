using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.Milktea.Service.BusinessModels
{
    public class ComboProductModel
    {
        public int ComboId { get; set; }

        public int ProductId { get; set; }

        public int? Quantity { get; set; }
    }
}
