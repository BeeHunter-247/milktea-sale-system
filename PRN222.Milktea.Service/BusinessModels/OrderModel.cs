using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.Milktea.Service.BusinessModels
{
    public class OrderModel
    {
		public int OrderId { get; set; }

		public int AccountId { get; set; }

		public string CustomerName { get; set; }

		public DateTime? OrderDate { get; set; }

		public decimal TotalAmount { get; set; }

		public string Status { get; set; }
	}
}
