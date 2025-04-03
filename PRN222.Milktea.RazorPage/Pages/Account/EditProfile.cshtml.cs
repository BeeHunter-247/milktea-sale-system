using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN222.Milktea.Service.BusinessModels;
using PRN222.Milktea.Service.Services.Interfaces;
using System.Security.Claims;

namespace PRN222.Milktea.RazorPage.Pages.Account
{
    public class EditProfileModel : PageModel
    {
        private readonly ICustomerService _customerService;

        public EditProfileModel(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [BindProperty]
        public CustomerProfile Profile { get; set; }

        public async Task OnGetAsync()
        {
            var accountId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            Profile = await _customerService.GetProfileAsync(accountId);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                Profile.AccountId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                await _customerService.UpdateProfileAsync(Profile);
                return RedirectToPage("Profile");
            }
            return Page();
        }
    }
}
