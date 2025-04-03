using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using PRN222.Milktea.Service.BusinessModels;
using PRN222.Milktea.Service.Services.Interfaces;
using System.Linq;
using System.Security.Claims;

namespace PRN222.Milktea.RazorPage.Pages.Cart
{
    public class CheckoutModel : PageModel
    {
        private readonly ICustomerService _customerService;
        private readonly IHubContext<PRN222.Milktea.RazorPage.Hubs.OrderHub> _orderHub;
        private const string CartCookieName = "Cart"; // Tên cookie để lưu trữ giỏ hàng

        public CheckoutModel(ICustomerService customerService, IHubContext<PRN222.Milktea.RazorPage.Hubs.OrderHub> orderHub)
        {
            _customerService = customerService;
            _orderHub = orderHub;
        }

        public CartViewModel Cart { get; set; }

        // Sử dụng cookie thay vì TempData
        public void OnGet()
        {
            // Lấy giỏ hàng từ cookie
            var cartCookie = Request.Cookies[CartCookieName];
            if (cartCookie != null)
            {
                Cart = JsonConvert.DeserializeObject<CartViewModel>(cartCookie);
            }
            else
            {
                Cart = new CartViewModel(); // Nếu không có giỏ hàng, tạo giỏ hàng mới
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Lấy giỏ hàng từ cookie
            var cartCookie = Request.Cookies[CartCookieName];
            if (cartCookie == null)
            {
                return RedirectToPage("/Cart/Index"); 
            }

            var cart = JsonConvert.DeserializeObject<CartViewModel>(cartCookie);
            if (cart == null || !cart.Items.Any())
            {
                return RedirectToPage("/Cart/Index"); 
            }

            var accountId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Tạo đơn hàng
            var order = await _customerService.CreateOrderAsync(accountId, cart);

            // Tạo thanh toán cho đơn hàng vừa tạo
            var paymentMethod = "Credit Card"; // Ví dụ: bạn có thể lấy từ UI hoặc từ hệ thống thanh toán
            var payment = await _customerService.CreatePaymentAsync(order.OrderId, paymentMethod);

            // Cập nhật trạng thái của đơn hàng và thanh toán qua SignalR
            await _orderHub.Clients.All.SendAsync("ReceiveOrderUpdate", order.OrderId, order.Status);

            Response.Cookies.Delete(CartCookieName);

            return RedirectToPage("/Orders/Index");
        }
    }
}
