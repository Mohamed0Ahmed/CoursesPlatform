

using System.Linq.Expressions;

namespace Courses.Infrastructure.Data.Repositories
{
    public class Repository<T, TKey> : IRepository<T, TKey> where T : BaseEntity<TKey> where TKey : IEquatable<TKey>
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        // Read Operations
        public async Task<T?> GetByIdAsync(TKey id, bool includeDeleted = false, bool onlyDeleted = false)
        {
            var query = _dbSet.AsQueryable();

            if (!includeDeleted && !onlyDeleted)
                query = query.Where(e => !e.IsDeleted);
            else if (onlyDeleted)
                query = query.Where(e => e.IsDeleted);

            return await query.FirstOrDefaultAsync(e => e.Id.Equals(id));
        }

        public async Task<T?> GetByIdWithIncludesAsync(TKey id, bool includeDeleted = false, bool onlyDeleted = false, Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            var query = _dbSet.AsQueryable();

            if (!includeDeleted && !onlyDeleted)
                query = query.Where(e => !e.IsDeleted);
            else if (onlyDeleted)
                query = query.Where(e => e.IsDeleted);

            if (include != null)
                query = include(query);

            return await query.FirstOrDefaultAsync(e => e.Id.Equals(id));
        }

        public async Task<IEnumerable<T>> GetAllAsync(bool includeDeleted = false, bool onlyDeleted = false)
        {
            var query = _dbSet.AsQueryable();

            if (!includeDeleted && !onlyDeleted)
                query = query.Where(e => !e.IsDeleted);
            else if (onlyDeleted)
                query = query.Where(e => e.IsDeleted);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllWithIncludesAsync(bool includeDeleted = false, bool onlyDeleted = false, Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            var query = _dbSet.AsQueryable();

            if (!includeDeleted && !onlyDeleted)
                query = query.Where(e => !e.IsDeleted);
            else if (onlyDeleted)
                query = query.Where(e => e.IsDeleted);

            if (include != null)
                query = include(query);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, bool includeDeleted = false, bool onlyDeleted = false)
        {
            var query = _dbSet.AsQueryable();

            if (!includeDeleted && !onlyDeleted)
                query = query.Where(e => !e.IsDeleted);
            else if (onlyDeleted)
                query = query.Where(e => e.IsDeleted);

            return await query.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<T>> FindWithIncludesAsync(Expression<Func<T, bool>> predicate, bool includeDeleted = false, bool onlyDeleted = false, Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            var query = _dbSet.AsQueryable();

            if (!includeDeleted && !onlyDeleted)
                query = query.Where(e => !e.IsDeleted);
            else if (onlyDeleted)
                query = query.Where(e => e.IsDeleted);

            query = query.Where(predicate);

            if (include != null)
                query = include(query);

            return await query.ToListAsync();
        }

        // Query Operations
        public async Task<bool> AnyAsync(Expression<Func<T, bool>>? predicate = null, bool includeDeleted = false, bool onlyDeleted = false)
        {
            var query = _dbSet.AsQueryable();

            if (!includeDeleted && !onlyDeleted)
                query = query.Where(e => !e.IsDeleted);
            else if (onlyDeleted)
                query = query.Where(e => e.IsDeleted);

            if (predicate != null)
                query = query.Where(predicate);

            return await query.AnyAsync();
        }

        public async Task<bool> ExistsAsync(TKey id, bool includeDeleted = false, bool onlyDeleted = false)
        {
            var query = _dbSet.AsQueryable();

            if (!includeDeleted && !onlyDeleted)
                query = query.Where(e => !e.IsDeleted);
            else if (onlyDeleted)
                query = query.Where(e => e.IsDeleted);

            return await query.AnyAsync(e => e.Id.Equals(id));
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, bool includeDeleted = false, bool onlyDeleted = false)
        {
            var query = _dbSet.AsQueryable();

            if (!includeDeleted && !onlyDeleted)
                query = query.Where(e => !e.IsDeleted);
            else if (onlyDeleted)
                query = query.Where(e => e.IsDeleted);

            if (predicate != null)
                query = query.Where(predicate);

            return await query.CountAsync();
        }

        // Create Operations
        public async Task<T> AddAsync(T entity)
        {
            var result = await _dbSet.AddAsync(entity);
            return result.Entity;
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            return entities;
        }

        // Update Operations
        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        // Delete Operations
        public void Delete(T entity)
        {
            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;
            _dbSet.Update(entity);
        }

        public void DeleteHard(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                entity.IsDeleted = true;
                entity.DeletedAt = DateTime.UtcNow;
            }
            _dbSet.UpdateRange(entities);
        }

        public async Task DeleteAsync(TKey id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
                Delete(entity);
        }

        public async Task DeleteHardAsync(TKey id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
                DeleteHard(entity);
        }

        // Restore Operations
        public async Task RestoreAsync(TKey id)
        {
            var entity = await GetByIdAsync(id, includeDeleted: true, onlyDeleted: true);
            if (entity != null)
            {
                entity.IsDeleted = false;
                entity.DeletedAt = null;
                entity.DeletedBy = null;
                Update(entity);
            }
        }

        public void RestoreRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                entity.IsDeleted = false;
                entity.DeletedAt = null;
                entity.DeletedBy = null;
            }
            UpdateRange(entities);
        }
    }
}