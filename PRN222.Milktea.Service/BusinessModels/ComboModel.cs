using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.Milktea.Service.BusinessModels
{
    public class ComboModel
    {
        public int ComboId { get; set; }

        public string Name { get; set; }

        public decimal? Discount { get; set; }

        public string Description { get; set; }

        public bool? IsActive { get; set; }
    }
}
