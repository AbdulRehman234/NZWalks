using Microsoft.EntityFrameworkCore;
using NZWalks.API.Interfaces.Generic;

namespace NZWalks.API.Repositories.Generic
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();

        }
        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> DeleteAsync(Guid id)
        {
            var domainEntity = await this.GetByIdAsync(id);
            if (domainEntity != null)
            {
               _dbSet.Remove(domainEntity);
               await _context.SaveChangesAsync();
                return domainEntity;
            }
            return null;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
          return  await _dbSet.FindAsync(id);
        }
        public async Task<T> UpdateAsync(Guid id ,T entity)
        {
            var domainEntity = this.GetByIdAsync(id);
            if (domainEntity != null)
            {

                _dbSet.Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
                return entity;
            }
            return entity;
        }

        public IQueryable<T> Query()
        {
            return _dbSet.AsQueryable();
        }

    }
}
