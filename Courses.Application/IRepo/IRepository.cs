
namespace Courses.Application.IRepo
{
    public interface IRepository<T, TKey> where T : BaseEntity<TKey> where TKey : IEquatable<TKey>
    {
        // Read Operations
        Task<T?> GetByIdAsync(TKey id, bool includeDeleted = false, bool onlyDeleted = false);
        Task<T?> GetByIdWithIncludesAsync(TKey id, bool includeDeleted = false, bool onlyDeleted = false, Func<IQueryable<T>, IQueryable<T>>? include = null);
        Task<IEnumerable<T>> GetAllAsync(bool includeDeleted = false, bool onlyDeleted = false);
        Task<IEnumerable<T>> GetAllWithIncludesAsync(bool includeDeleted = false, bool onlyDeleted = false, Func<IQueryable<T>, IQueryable<T>>? include = null);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, bool includeDeleted = false, bool onlyDeleted = false);
        Task<IEnumerable<T>> FindWithIncludesAsync(Expression<Func<T, bool>> predicate, bool includeDeleted = false, bool onlyDeleted = false, Func<IQueryable<T>, IQueryable<T>>? include = null);

        // Query Operations
        Task<bool> AnyAsync(Expression<Func<T, bool>>? predicate = null, bool includeDeleted = false, bool onlyDeleted = false);
        Task<bool> ExistsAsync(TKey id, bool includeDeleted = false, bool onlyDeleted = false);
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, bool includeDeleted = false, bool onlyDeleted = false);

        // Create Operations
        Task<T> AddAsync(T entity);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);

        // Update Operations
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);

        // Delete Operations
        void Delete(T entity);
        void DeleteHard(T entity);
        void DeleteRange(IEnumerable<T> entities);
        Task DeleteAsync(TKey id);
        Task DeleteHardAsync(TKey id);

        // Restore Operations
        Task RestoreAsync(TKey id);
        void RestoreRange(IEnumerable<T> entities);
    }
}
