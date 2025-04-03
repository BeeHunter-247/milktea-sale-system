using PRN222.Milktea.Repository.Models;
using PRN222.Milktea.Service.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.Milktea.Service.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerProfile> GetProfileAsync(int id);
        Task UpdateProfileAsync(CustomerProfile profile);
        Task<OrderViewModel> CreateOrderAsync(int accountId, CartViewModel cart);
        Task<IEnumerable<OrderViewModel>> GetOrderHistoryAsync(int accountId);
        Task CancelOrderAsync(int orderId);

        Task<PaymentViewModel> CreatePaymentAsync(int orderId, string paymentMethod);
        Task<PaymentViewModel> MakePaymentAsync(int orderId, string paymentMethod);
        Task<IEnumerable<PaymentViewModel>> GetPaymentsAsync(int accountId);
        Task<Account> AuthenticateAsync(string email, string password);

    }
}
