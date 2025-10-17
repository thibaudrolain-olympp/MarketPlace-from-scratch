using Microsoft.EntityFrameworkCore;

namespace Marketplace.Repositories
{
    /// <inheritdoc/>
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly MarketplaceDbContext _db;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(MarketplaceDbContext db)
        {
            _db = db;
            _dbSet = db.Set<T>();
        }

        public async Task<IList<T>> GetAllAsync() => await _dbSet.AsNoTracking().ToListAsync();

        public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

        public async Task<T> AddAsync(T entity)
        {
            _dbSet.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<T?> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) return false;
            _dbSet.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}