using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN222.Milktea.Service.BusinessModels;
using PRN222.Milktea.Service.Services.Interfaces;
using System.Security.Claims;

namespace PRN222.Milktea.RazorPage.Pages.Payments
{
    public class PaymentsIndexModel : PageModel
    {
        private readonly IPaymentService _paymentService;
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;

        public PaymentsIndexModel(ICustomerService customerService, IPaymentService paymentService, IOrderService orderService)
        {
            _customerService = customerService;
            _paymentService = paymentService;
            _orderService = orderService;
        }

        public IEnumerable<PaymentViewModel> Payments { get; set; }

        public async Task OnGetAsync()
        {
            var accountId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            Payments = await _customerService.GetPaymentsAsync(accountId);
        }

        public async Task<IActionResult> OnPostPayAsync(int orderId, string paymentMethod)
        {
            var payment = await _paymentService.GetPaymentDetailsAsync(orderId);

            if (payment == null)
            {
                ModelState.AddModelError(string.Empty, "Payment record not found.");
                return Page();
            }

            var methodSuccess = await _paymentService.UpdatePaymentMethodAsync(payment.PaymentId, paymentMethod);
            if (!methodSuccess)
            {
                ModelState.AddModelError(string.Empty, "Failed to update payment method.");
                return Page();
            }

            var success = await _paymentService.UpdatePaymentStatusAsync(payment.PaymentId, "Completed");
            if (!success)
            {
                ModelState.AddModelError(string.Empty, "Failed to update payment status.");
            }

            var orderSuccess = await _orderService.UpdateOrderStatusAsync(orderId, "Completed");
            if (!orderSuccess)
            {
                ModelState.AddModelError(string.Empty, "Failed to update order status.");
                return Page();
            }

            return RedirectToPage();
        }
    }
}
