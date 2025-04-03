// PRN222.Milktea.Service/Services/CustomerService.cs
using PRN222.Milktea.Repository.Models;
using PRN222.Milktea.Repository.UnitOfWork;
using PRN222.Milktea.Service.BusinessModels;
using PRN222.Milktea.Service.Services.Interfaces;

namespace PRN222.Milktea.Service.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<Account> AuthenticateAsync(string email, string password)
        {
            var accounts = await _unitOfWork.AccountRepository.GetAsync();
            return accounts.FirstOrDefault(a => a.Email == email && a.AccountPassword == password && a.AccountRole == 2);
        }

        public async Task<CustomerProfile> GetProfileAsync(int accountId)
        {
            var account = await _unitOfWork.AccountRepository.GetByIdAsync(accountId);
            if (account == null) throw new Exception("Account not found");
            return new CustomerProfile
            {
                AccountId = account.AccountId,
                FullName = account.FullName,
                Email = account.Email
            };
        }

        public async Task UpdateProfileAsync(CustomerProfile profile)
        {
            var account = await _unitOfWork.AccountRepository.GetByIdAsync(profile.AccountId);
            if (account == null) throw new Exception("Account not found");
            account.FullName = profile.FullName;
            account.Email = profile.Email;
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<OrderViewModel> CreateOrderAsync(int accountId, CartViewModel cart)
        {
            var order = new Order
            {
                AccountId = accountId,
                CustomerName = (await _unitOfWork.AccountRepository.GetByIdAsync(accountId))?.FullName ?? throw new Exception("Account not found"),
                TotalAmount = cart.Items.Sum(i => i.UnitPrice * i.Quantity),
                Status = "Pending",
                OrderDate = DateTime.Now 
            };
            await _unitOfWork.OrderRepository.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            foreach (var item in cart.Items)
            {
                await _unitOfWork.OrderDetailRepository.AddAsync(new OrderDetail
                {
                    OrderId = order.OrderId,
                    ProductId = item.ProductId,
                    ComboId = item.ComboId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                });
            }
            await _unitOfWork.SaveChangesAsync();

            return new OrderViewModel
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status
            };
        }

        public async Task<IEnumerable<OrderViewModel>> GetOrderHistoryAsync(int accountId)
        {
            var orders = await _unitOfWork.OrderRepository.GetAsync();
            return orders.Where(o => o.AccountId == accountId).Select(o => new OrderViewModel
            {
                OrderId = o.OrderId,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                Details = o.OrderDetails.Select(d => new OrderDetailViewModel
                {
                    ProductName = d.ProductId.HasValue ? d.Product.Name : d.Combo?.ComboName,
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice
                }).ToList()
            });
        }

        public async Task CancelOrderAsync(int orderId)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(orderId);
            if (order == null) throw new Exception("Order not found");
            if (order.Status == "Pending")
            {
                order.Status = "Cancelled";
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<PaymentViewModel> CreatePaymentAsync(int orderId, string paymentMethod)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(orderId);
            if (order == null) throw new Exception("Order not found");
            var payment = new Payment
            {
                OrderId = orderId,
                Amount = order.TotalAmount,
                PaymentMethod = paymentMethod,
                PaymentDate = DateTime.Now, 
                Status = "Pending" 
            };
            await _unitOfWork.PaymentRepository.AddAsync(payment);
            await _unitOfWork.SaveChangesAsync();

            return new PaymentViewModel
            {
                OrderId = payment.OrderId,
                PaymentDate = payment.PaymentDate,
                Amount = payment.Amount,
                PaymentMethod = payment.PaymentMethod,
                Status = payment.Status
            };
        }

        public async Task<PaymentViewModel> MakePaymentAsync(int orderId, string paymentMethod)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(orderId);
            if (order == null) throw new Exception("Order not found");
            var payment = new Payment
            {
                OrderId = orderId,
                Amount = order.TotalAmount,
                PaymentMethod = paymentMethod,
                PaymentDate = DateTime.Now, 
                Status = "Completed" 
            };
            await _unitOfWork.PaymentRepository.AddAsync(payment);
            await _unitOfWork.SaveChangesAsync();

            return new PaymentViewModel
            {
                OrderId = payment.OrderId,
                PaymentDate = payment.PaymentDate,
                Amount = payment.Amount,
                PaymentMethod = payment.PaymentMethod,
                Status = payment.Status
            };
        }

        public async Task<IEnumerable<PaymentViewModel>> GetPaymentsAsync(int accountId)
        {
            var orders = await _unitOfWork.OrderRepository.GetAsync();
            var orderIds = orders.Where(o => o.AccountId == accountId).Select(o => o.OrderId);
            var payments = await _unitOfWork.PaymentRepository.GetAsync();
            return payments.Where(p => orderIds.Contains(p.OrderId)).Select(p => new PaymentViewModel
            {
                OrderId = p.OrderId,
                PaymentDate = p.PaymentDate, // No null check needed if DB default is set
                Amount = p.Amount,
                PaymentMethod = p.PaymentMethod,
                Status = p.Status
            });
        }
    }
}