using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.Milktea.Service.BusinessModels
{
    public class ExtraModel
    {
        public int ExtraId { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public bool? IsActive { get; set; }
    }
}
