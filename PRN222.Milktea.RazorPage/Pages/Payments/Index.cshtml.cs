using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN222.Milktea.Service.BusinessModels;
using PRN222.Milktea.Service.Services.Interfaces;
using System.Security.Claims;

namespace PRN222.Milktea.RazorPage.Pages.Payments
{
    public class PaymentsIndexModel : PageModel
    {
        private readonly ICustomerService _customerService;

        public PaymentsIndexModel(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public IEnumerable<PaymentViewModel> Payments { get; set; }

        public async Task OnGetAsync()
        {
            var accountId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            Payments = await _customerService.GetPaymentsAsync(accountId);
        }

        public async Task<IActionResult> OnPostPayAsync(int orderId, string paymentMethod)
        {
            await _customerService.MakePaymentAsync(orderId, paymentMethod);
            return RedirectToPage();
        }
    }
}
