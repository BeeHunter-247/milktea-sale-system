using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN222.Milktea.Service.BusinessModels;
using PRN222.Milktea.Service.Services.Interfaces;
using System.Threading.Tasks;

namespace PRN222.Milktea.RazorPage.Pages.Orders
{
    public class OrderDetailsModel : PageModel
    {
        private readonly IOrderService _orderService;

        public OrderDetailsModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // Đơn hàng và các chi tiết đơn hàng sẽ được khởi tạo tại đây
        public OrderViewModel Order { get; set; }
        public IEnumerable<OrderDetailViewModel> OrderItems { get; set; }

        // Lấy thông tin đơn hàng và các chi tiết của nó
        public async Task OnGetAsync(int orderId)
        {
            // Lấy thông tin đơn hàng
            Order = await _orderService.GetOrderAsync(orderId);

            // Lấy chi tiết các mặt hàng trong đơn hàng
            OrderItems = await _orderService.GetOrderDetailsAsync(orderId);
        }
    }
}
