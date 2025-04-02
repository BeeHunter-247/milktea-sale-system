using PRN222.Milktea.Repository.Models;

namespace PRN222.Milktea.MVC.Models
{
    public class DashboardViewModel
    {
        public int TotalRegisteredAccounts { get; set; }
        public int TotalBannedAccounts { get; set; }
        public List<Category> Categories { get; set; }
        public List<MonthlyRevenue> TotalRevenue { get; set; } 
    }

    public class MonthlyRevenue
    {
        public DateTime Month { get; set; }
        public decimal Total { get; set; }
    }
}
