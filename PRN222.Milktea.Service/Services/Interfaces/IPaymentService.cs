using PRN222.Milktea.Service.BusinessModels;
using System.Threading.Tasks;

namespace PRN222.Milktea.Service.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentViewModel> GetPaymentDetailsAsync(int orderId);
        Task<bool> ProcessPaymentAsync(PaymentModel payment);
        Task<bool> UpdatePaymentStatusAsync(int paymentId, string status);

        Task<bool> UpdatePaymentMethodAsync(int paymentId, string paymentMethod);
    }
}
