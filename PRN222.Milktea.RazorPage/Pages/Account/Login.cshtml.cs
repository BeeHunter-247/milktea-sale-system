using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN222.Milktea.Service.Services.Interfaces;
using System.Security.Claims;

namespace PRN222.Milktea.RazorPage.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly ICustomerService _customerService;

        public LoginModel(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Password { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var account = await _customerService.AuthenticateAsync(Email, Password);
            if (account != null && account.AccountRole == 2)
            {
                // Simulate authentication (replace with real auth)
                var claims = new[] { new Claim(ClaimTypes.NameIdentifier, account.AccountId.ToString()) };
                var identity = new ClaimsIdentity(claims, "CustomerAuth");
                await HttpContext.SignInAsync(new ClaimsPrincipal(identity));
                return RedirectToPage("/Products/Index");
            }
            ModelState.AddModelError("", "Invalid credentials");
            return Page();
        }
    }
    }
