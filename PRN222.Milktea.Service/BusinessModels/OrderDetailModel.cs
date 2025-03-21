using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.Milktea.Service.BusinessModels
{
    public class OrderDetailModel
    {
		public int OrderDetailId { get; set; }

		public int OrderId { get; set; }

		public int ProductId { get; set; }

		public int Quantity { get; set; }

		public decimal UnitPrice { get; set; }
	}
}
