using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PRN222.Milktea.Repository.UnitOfWork;
using PRN222.Milktea.Service.BusinessModels;
using PRN222.Milktea.Service.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace PRN222.Milktea.Service.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Lấy thông tin thanh toán theo OrderId
        public async Task<PaymentViewModel> GetPaymentDetailsAsync(int orderId)
        {
            try
            {
                var payment = (await _unitOfWork.PaymentRepository.GetByConditionAsync(p => p.OrderId == orderId))?.FirstOrDefault();
                if (payment == null)
                {
                    return null;
                }

                return _mapper.Map<PaymentViewModel>(payment);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching payment details for order ID {orderId}: {ex.Message}");
            }
        }

        // Xử lý thanh toán
        public async Task<bool> ProcessPaymentAsync(PaymentModel payment)
        {
            try
            {
                var paymentEntity = _mapper.Map<PRN222.Milktea.Repository.Models.Payment>(payment);

                // Giả sử bạn có phương thức để lưu thanh toán vào cơ sở dữ liệu
                await _unitOfWork.PaymentRepository.AddAsync(paymentEntity);
                await _unitOfWork.SaveChangesAsync();  // Thay vì CommitAsync, gọi SaveChangesAsync

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error processing payment: {ex.Message}");
            }
        }

        // Cập nhật trạng thái thanh toán
        public async Task<bool> UpdatePaymentStatusAsync(int paymentId, string status)
        {
            try
            {
                var payment = await _unitOfWork.PaymentRepository.GetByIdAsync(paymentId);
                if (payment == null)
                {
                    return false;
                }

                payment.Status = status;

                _unitOfWork.PaymentRepository.Update(payment);
                await _unitOfWork.SaveChangesAsync();  

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating payment status for payment ID {paymentId}: {ex.Message}");
            }
        }

        public async Task<bool> UpdatePaymentMethodAsync(int paymentId, string paymentMethod)
        {
            try
            {
                var payment = await _unitOfWork.PaymentRepository.GetByIdAsync(paymentId);
                if (payment == null)
                {
                    return false; // Nếu không tìm thấy payment, trả về false
                }

                payment.PaymentMethod = paymentMethod; // Cập nhật phương thức thanh toán

                _unitOfWork.PaymentRepository.Update(payment); // Cập nhật lại trong cơ sở dữ liệu
                await _unitOfWork.SaveChangesAsync(); // Lưu thay đổi

                return true; // Thành công
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating payment method for payment ID {paymentId}: {ex.Message}");
            }
        }

    }
}
