using Microsoft.EntityFrameworkCore;
using PRN222.Milktea.Repository.Models;
using System.Linq.Expressions;

namespace PRN222.Milktea.Repository.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly MilkteaSaleDBContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(MilkteaSaleDBContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null)
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();

            if(include != null)
            {
                query = include(query);
            }

            return await query.FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<TEntity>> GetAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>?> GetByConditionAsync(Expression<Func<TEntity, bool>> condition = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();

            if(condition != null)
            {
                query = query.Where(condition);
            }

            if(include != null)
            {
                query = include(query);
            }

            if(orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }
    }
}
