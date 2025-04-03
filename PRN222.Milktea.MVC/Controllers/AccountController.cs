using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN222.Milktea.MVC.Models;
using PRN222.Milktea.Service.Services.Interfaces;
using System.Security.Claims;

namespace PRN222.Milktea.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public string ErrorMessage { get; set; }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Products");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Login");
            }

            var account = await _accountService.Login(loginViewModel.Email, loginViewModel.Password);

            if (account == null)
            {
                ErrorMessage = "Invalid email or password. Please try again!";
                return NotFound();
            }

            if (account.AccountRole != 1)
            {
                return RedirectToAction("AccessDenied");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.AccountId.ToString()),
                new Claim(ClaimTypes.Email, account.Email),
                new Claim(ClaimTypes.Role, account.AccountRole.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(10),
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            return RedirectToAction("Index", "Products");
        }

        [HttpGet]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Index()
        {
            var accountIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (accountIdClaim == null)
            {
                return Unauthorized();
            }

            int currentAdminId = int.Parse(accountIdClaim.Value);

            var accounts = await _accountService.GetAllAccountsExceptAdminAsync(currentAdminId);
            return View(accounts);
        }

        [HttpPost]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Ban(int accountId)
        {
            await _accountService.BanAccountAsync(accountId);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}