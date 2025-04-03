using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using PRN222.Milktea.Service.BusinessModels;
using PRN222.Milktea.Service.Services.Interfaces;
using System.Security.Claims;

namespace PRN222.Milktea.RazorPage.Pages.Orders
{
    public class OrdersIndexModel : PageModel
    {
        private readonly ICustomerService _customerService;
        private readonly IHubContext<PRN222.Milktea.RazorPage.Hubs.OrderHub> _orderHub;

        public OrdersIndexModel(ICustomerService customerService, IHubContext<PRN222.Milktea.RazorPage.Hubs.OrderHub> orderHub)
        {
            _customerService = customerService;
            _orderHub = orderHub;
        }

        public IEnumerable<OrderViewModel> Orders { get; set; }

        public async Task OnGetAsync()
        {
            var accountId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            Orders = await _customerService.GetOrderHistoryAsync(accountId);
        }

        public async Task<IActionResult> OnPostCancelAsync(int orderId)
        {
            await _customerService.CancelOrderAsync(orderId);
            await _orderHub.Clients.All.SendAsync("ReceiveOrderUpdate", orderId, "Cancelled");
            return RedirectToPage();
        }
    }
}
