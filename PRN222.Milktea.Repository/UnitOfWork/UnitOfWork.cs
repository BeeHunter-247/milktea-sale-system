using PRN222.Milktea.Repository.Models;
using PRN222.Milktea.Repository.Repositories;

namespace PRN222.Milktea.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MilkteaSaleDBContext _context;
        private IGenericRepository<Account> _accountRepository;
        private IGenericRepository<Combo> _comboRepository;
        private IGenericRepository<ComboProduct> _comboProductRepository;
        private IGenericRepository<Extra> _extraRepository;
        private IGenericRepository<Order> _orderRepository;
        private IGenericRepository<OrderDetail> _orderDetailRepository;
        private IGenericRepository<Product> _productRepository;

        public UnitOfWork(MilkteaSaleDBContext context)
        {
            _context = context;
        }

        public IGenericRepository<Account> AccountRepository
        {
            get { return _accountRepository ??= new GenericRepository<Account>(_context); }
        }

        public IGenericRepository<Combo> ComboRepository
        {
            get { return _comboRepository ??= new GenericRepository<Combo>(_context); }
        }

        public IGenericRepository<ComboProduct> ComboProductRepository
        {
            get { return _comboProductRepository ??= new GenericRepository<ComboProduct>(_context); }
        }

        public IGenericRepository<Extra> ExtraRepository
        {
            get { return _extraRepository ??= new GenericRepository<Extra>(_context); }
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