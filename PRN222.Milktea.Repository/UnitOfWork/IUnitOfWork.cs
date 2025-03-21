using PRN222.Milktea.Repository.Models;
using PRN222.Milktea.Repository.Repositories;

namespace PRN222.Milktea.Repository.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Account> AccountRepository { get; }
        IGenericRepository<ComboProduct> ComboProductRepository { get; }
        IGenericRepository<Order> OrderRepository { get; }
        IGenericRepository<OrderDetail> OrderDetailRepository { get; }
        IGenericRepository<Product> ProductRepository { get; }
        IGenericRepository<Payment> PaymentRepository { get; }
        IGenericRepository<Category> CategoryRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
