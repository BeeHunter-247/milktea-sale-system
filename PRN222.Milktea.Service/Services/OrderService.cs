using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PRN222.Milktea.Repository.UnitOfWork;
using PRN222.Milktea.Service.BusinessModels;
using PRN222.Milktea.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRN222.Milktea.Service.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<OrderViewModel> GetOrderAsync(int orderId)
        {
            try
            {
                var order = await _unitOfWork.OrderRepository.GetByIdAsync(orderId);
                if (order == null)
                {
                    return null;
                }

                return _mapper.Map<OrderViewModel>(order);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching order with ID {orderId}: {ex.Message}");
            }
        }

        public async Task<IEnumerable<OrderDetailViewModel>> GetOrderDetailsAsync(int orderId)
        {
            try
            {
                var orderDetails = await _unitOfWork.OrderDetailRepository.GetByConditionAsync(
                    od => od.OrderId == orderId,
                    include: od => od.Include(od => od.Product) 
                );

                var orderDetailViewModels = orderDetails.Select(od => new OrderDetailViewModel
                {
                    ProductName = od.Product.Name, 
                    Quantity = od.Quantity,
                    UnitPrice = od.UnitPrice
                }).ToList();

                return orderDetailViewModels;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching order details for order ID {orderId}: {ex.Message}");
            }
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, string status)
        {
            try
            {
                var order = await _unitOfWork.OrderRepository.GetByIdAsync(orderId);
                if (order == null)
                {
                    return false; // Đơn hàng không tồn tại
                }

                order.Status = status; // Cập nhật trạng thái

                _unitOfWork.OrderRepository.Update(order);
                await _unitOfWork.SaveChangesAsync();

                return true; // Thành công
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating order status for order ID {orderId}: {ex.Message}");
            }
        }

    }
}
