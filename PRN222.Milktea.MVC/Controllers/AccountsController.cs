using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN222.Milktea.Service.Services;
using PRN222.Milktea.Service.Services.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PRN222.Milktea.MVC.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        public async Task<IActionResult> Index()
        {
            // Get the AccountId or Email from claims
            var accountIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);  // AccountId
            var emailClaim = User.FindFirst(ClaimTypes.Email); // Email

            if (accountIdClaim == null || emailClaim == null)
            {
                // Handle the case where the claim is not found
                return Unauthorized();  // Or another appropriate response
            }

            int currentAdminId = int.Parse(accountIdClaim.Value); // Get AccountId from claims

            // You can use the email as well if needed
            string userEmail = emailClaim.Value;  // Get email from claims

            var accounts = await _accountService.GetAllAccountsExceptAdminAsync(currentAdminId);
            return View(accounts);
        }

        [HttpPost]
        public async Task<IActionResult> Ban(int accountId)
        {
            await _accountService.BanAccountAsync(accountId);
            return RedirectToAction("Index");
        }
    }
}