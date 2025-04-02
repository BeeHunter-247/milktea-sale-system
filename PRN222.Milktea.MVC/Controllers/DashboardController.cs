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
                ))
                .Where(o => o.OrderDate.HasValue)  // Make sure OrderDate is not null
                .GroupBy(o => new { o.OrderDate.Value.Year, o.OrderDate.Value.Month })  // Access Year and Month using Value
                .Select(g => new MonthlyRevenue
                {
                    Month = new DateTime(g.Key.Year, g.Key.Month, 1),
                    Total = g.Sum(o => o.TotalAmount)
                }).ToList()
            };

            return View(model);
        }


    }
}