using PRN222.Milktea.Repository.Models;
using PRN222.Milktea.Repository.Repositories;

namespace PRN222.Milktea.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MilkteaSaleDBContext _context;
        private IGenericRepository<Account> _accountRepository;
        private IGenericRepository<Combo> _comboRepository;
        private IGenericRepository<Order> _orderRepository;
        private IGenericRepository<OrderDetail> _orderDetailRepository;
        private IGenericRepository<Product> _productRepository;
        private IGenericRepository<Payment> _paymentRepository;
        private IGenericRepository<Category> _categoryRepository;

        public UnitOfWork(MilkteaSaleDBContext context)
        {
            _context = context;
        }

        public IGenericRepository<Account> AccountRepository
        {
            get { return _accountRepository ??= new GenericRepository<Account>(_context); }
        }

        public IGenericRepository<Payment> PaymentRepository
        {
            get { return _paymentRepository ??= new GenericRepository<Payment>(_context); }
        }

        public IGenericRepository<Combo> ComboRepository
        {
            get { return _comboRepository ??= new GenericRepository<Combo>(_context); }
        }

        public IGenericRepository<Category> CategoryRepository
        {
            get { return _categoryRepository ??= new GenericRepository<Category>(_context); }
        }

        public IGenericRepository<Order> OrderRepository
        {
            get { return _orderRepository ??= new GenericRepository<Order>(_context); }
        }

        public IGenericRepository<OrderDetail> OrderDetailRepository
        {
            get { return _orderDetailRepository ??= new GenericRepository<OrderDetail>(_context); }
        }

        public IGenericRepository<Product> ProductRepository
        {
            get { return _productRepository ??= new GenericRepository<Product>(_context); }
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}