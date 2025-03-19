using PRN222.Milktea.Repository.Models;
using PRN222.Milktea.Repository.Repositories;

namespace PRN222.Milktea.Repository.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Account> AccountRepository { get; }
        IGenericRepository<Combo> ComboRepository { get; }
        IGenericRepository<ComboProduct> ComboProductRepository { get; }
        IGenericRepository<Extra> ExtraRepository { get; }
        IGenericRepository<Order> OrderRepository { get; }
        IGenericRepository<OrderDetail> OrderDetailRepository { get; }
        IGenericRepository<Product> ProductRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
