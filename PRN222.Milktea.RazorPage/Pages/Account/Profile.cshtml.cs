using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN222.Milktea.Service.BusinessModels;
using PRN222.Milktea.Service.Services.Interfaces;
using System.Security.Claims;

namespace PRN222.Milktea.RazorPage.Pages.Account
{
    public class ProfileModel : PageModel
    {
        private readonly ICustomerService _customerService;

        public ProfileModel(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public CustomerProfile Profile { get; set; }

        public async Task OnGetAsync()
        {
            var accountId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            Profile = await _customerService.GetProfileAsync(accountId);
        }
    }
}
