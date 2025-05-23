﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.Milktea.Service.BusinessModels
{
	public class ProductModel
	{
		public int ProductId { get; set; }

		public int CategoryId { get; set; }

		public string Name { get; set; }

		public decimal Price { get; set; }

		public string Description { get; set; }

		public string Image { get; set; }

		public int ProductType { get; set; }

		public bool? IsActive { get; set; }

		public string? CategoryName { get; set; }
	}
    public class ProductViewModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string CategoryName { get; set; }
    }
}
