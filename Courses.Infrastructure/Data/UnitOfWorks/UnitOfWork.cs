using Courses.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Courses.Infrastructure.Data.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly Dictionary<Type, object> _repositories;
        private IDbContextTransaction? _currentTransaction;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();
        }

        // Repository Management
        public IRepository<T, TKey> GetRepository<T, TKey>() where T : BaseEntity<TKey> where TKey : IEquatable<TKey>
        {
            var type = typeof(T);
            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = new Repository<T, TKey>(_context);
            }
            return (IRepository<T, TKey>)_repositories[type];
        }

        // Change Tracking
        public bool HasChanges => _context.ChangeTracker.HasChanges();

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        // Transaction Management
        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction != null)
                return;

            _currentTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.CommitAsync(cancellationToken);
                    await _currentTransaction.DisposeAsync();
                    _currentTransaction = null;
                }
            }
            catch
            {
                await RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.RollbackAsync(cancellationToken);
                    await _currentTransaction.DisposeAsync();
                    _currentTransaction = null;
                }
            }
            catch
            {
                // Log the rollback error but don't throw
                throw;
            }
        }

        public bool HasActiveTransaction => _currentTransaction != null;

        // Entity Management
        public void Detach<T, TKey>(T entity) where T : BaseEntity<TKey> where TKey : IEquatable<TKey>
        {
            _context.Entry(entity).State = EntityState.Detached;
        }

        public void DetachRange<T, TKey>(IEnumerable<T> entities) where T : BaseEntity<TKey> where TKey : IEquatable<TKey>
        {
            foreach (var entity in entities)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }
        }

        public void Attach<T, TKey>(T entity) where T : BaseEntity<TKey> where TKey : IEquatable<TKey>
        {
            _context.Attach(entity);
        }

        public void AttachRange<T, TKey>(IEnumerable<T> entities) where T : BaseEntity<TKey> where TKey : IEquatable<TKey>
        {
            _context.AttachRange(entities);
        }

        // State Management
        public void ResetState()
        {
            _context.ChangeTracker.Clear();
        }

        public async Task EnsureDeletedAsync(CancellationToken cancellationToken = default)
        {
            await _context.Database.EnsureDeletedAsync(cancellationToken);
        }

        public void Dispose()
        {
            _currentTransaction?.Dispose();
            _context.Dispose();
        }
    }
}