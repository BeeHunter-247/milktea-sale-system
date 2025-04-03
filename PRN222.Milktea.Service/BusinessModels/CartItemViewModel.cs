using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.Milktea.Service.BusinessModels
{
    public class CartItemViewModel
    {
        public int? ProductId { get; set; }
        public int? ComboId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
    public class CartViewModel
    {
        public List<CartItemViewModel> Items { get; set; } = new List<CartItemViewModel>();
    }
}
