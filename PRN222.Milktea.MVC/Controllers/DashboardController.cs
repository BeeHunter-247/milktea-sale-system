using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN222.Milktea.MVC.Models;
using PRN222.Milktea.Repository.UnitOfWork;
using PRN222.Milktea.Service.Services;
using PRN222.Milktea.Service.Services.Interfaces;
using System.Threading.Tasks;

namespace PRN222.Milktea.MVC.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IUnitOfWork _unitOfWork;

        public DashboardController(IAccountService accountService, IUnitOfWork unitOfWork)
        {
            _accountService = accountService;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var model = new DashboardViewModel
            {
                TotalRegisteredAccounts = await _accountService.GetTotalRegisteredAccountsAsync(),
                TotalBannedAccounts = await _accountService.GetTotalBannedAccountsAsync(),
                Categories = (await _unitOfWork.CategoryRepository.GetAsync()).ToList(),
                TotalRevenue = (await _unitOfWork.OrderRepository.GetByConditionAsync(
                    condition: o => o.Status == "Completed"
                )).Sum(o => o.TotalAmount)
            };
            return View(model);
        }
    }
}