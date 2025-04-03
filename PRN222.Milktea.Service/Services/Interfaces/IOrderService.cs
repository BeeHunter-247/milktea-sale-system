using PRN222.Milktea.Service.BusinessModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PRN222.Milktea.Service.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderViewModel> GetOrderAsync(int orderId);
        Task<IEnumerable<OrderDetailViewModel>> GetOrderDetailsAsync(int orderId);
        Task<bool> UpdateOrderStatusAsync(int orderId, string status);
    }
}
