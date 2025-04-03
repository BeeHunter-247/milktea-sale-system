using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.Milktea.Service.BusinessModels
{
	public class PaymentModel
	{
		public int PaymentId { get; set; }

		public int OrderId { get; set; }

		public DateTime? PaymentDate { get; set; }

		public decimal Amount { get; set; }

		public string PaymentMethod { get; set; }

		public string Status { get; set; }
	}
    public class PaymentViewModel
    {
        
		public int PaymentId { get; set; }
		public int OrderId { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
    }
}
