using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.Milktea.Service.BusinessModels
{
	public class CategoryModel
	{
		public int CategoryId { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public bool? IsActive { get; set; }
	}
}
