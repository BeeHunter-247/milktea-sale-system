using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.Milktea.Service.BusinessModels
{
    public class ComboProductModel
    {
		public int ComboProductId { get; set; }

		public int IncludedProductId { get; set; }

		public int? Quantity { get; set; }
	}
}
