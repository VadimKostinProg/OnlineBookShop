using BookShop.Core.Domain.Entities;
using BookShop.Core.Domain.RepositoryContracts;
using BookShop.Infrastructure.DataBaseContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookShop.Infrastructure.Repositories
{
    public class RepositoryBase : IRepository
    {
        private readonly ApplicationContext _dbContext;

        public RepositoryBase(ApplicationContext context)
        {
            _dbContext = context;
        }

        public async Task AddAsync<T>(T entity) where T : EntityBase
        {
            _dbContext.Set<T>().Add(entity);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync<T>(Guid id) where T : EntityBase
        {
            T? entity = await _dbContext.Set<T>().FindAsync(id);

            if(entity == null)
            {
                return false;
            }

            _dbContext.Set<T>().Remove(entity);

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public Task<bool> ExistsAsync<T>(Expression<Func<T, bool>> predicate) where T : EntityBase
        {
            return Task.FromResult(_dbContext.Set<T>().Any(predicate));
        }

        public async Task<T?> FirstOrDefaultAsync<T>(Expression<Func<T, bool>> predicate, params string[] includeStrings) where T : EntityBase
        {
            var query = (await this.GetAllAsync<T>(predicate: null, includeStrings)).AsQueryable();

            return query.FirstOrDefault(predicate);
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(Expression<Func<T, bool>>? predicate = null, params string[] includeStrings) where T : EntityBase
        {
            var query = _dbContext.Set<T>().AsNoTracking().AsQueryable();

            if(predicate != null)
            {
                query = query.Where(predicate);
            }

            foreach(string includeString in includeStrings)
            {
                query = query.Include(includeString);
            }

            return await query.ToListAsync();
        }

        public async Task<T?> GetByIdAsync<T>(Guid id, params string[] includeStrings) where T : EntityBase
        {
            var query = await this.GetAllAsync<T>(predicate: null, includeStrings);

            return query.FirstOrDefault(entity => entity.Id == id);
        }

        public async Task UpdateAsync<T>(T entity) where T : EntityBase
        {
            _dbContext.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
