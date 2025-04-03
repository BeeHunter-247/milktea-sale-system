using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using PRN222.Milktea.RazorPage.Extensions;
using PRN222.Milktea.Service.BusinessModels;
using PRN222.Milktea.Service.Services.Interfaces;
using System.Security.Claims;

namespace PRN222.Milktea.RazorPage.Pages.Cart
{
    public class CheckoutModel : PageModel
    {
        private readonly ICustomerService _customerService;
        private readonly IHubContext<PRN222.Milktea.RazorPage.Hubs.OrderHub> _orderHub;

        public CheckoutModel(ICustomerService customerService, IHubContext<PRN222.Milktea.RazorPage.Hubs.OrderHub> orderHub)
        {
            _customerService = customerService;
            _orderHub = orderHub;
        }

        public CartViewModel Cart { get; set; }

        public void OnGet()
        {
            Cart = HttpContext.Session.GetObjectFromJson<CartViewModel>("Cart") ?? new CartViewModel();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var cart = HttpContext.Session.GetObjectFromJson<CartViewModel>("Cart");
            var accountId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var order = await _customerService.CreateOrderAsync(accountId, cart);
            await _orderHub.Clients.All.SendAsync("ReceiveOrderUpdate", order.OrderId, order.Status);
            HttpContext.Session.Remove("Cart");
            return RedirectToPage("/Orders/Index");
        }
    }
}
