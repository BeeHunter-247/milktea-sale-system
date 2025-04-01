using PRN222.Milktea.Repository.Models;

namespace PRN222.Milktea.MVC.Models
{
    public class DashboardViewModel
    {
        public int TotalRegisteredAccounts { get; set; }
        public int TotalBannedAccounts { get; set; }
        public List<Category> Categories { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
