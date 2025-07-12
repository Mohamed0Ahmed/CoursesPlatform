using Courses.Application.IRepo;
using Courses.Shared.Base;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Courses.Application.IUnit
{
    public interface IUnitOfWork : IDisposable
    {
        // Repository Management
        IRepository<T, TKey> GetRepository<T, TKey>() where T : BaseEntity<TKey> where TKey : IEquatable<TKey>;

        // Change Tracking
        bool HasChanges { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);

        // Transaction Management
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
        bool HasActiveTransaction { get; }

        // Entity Management
        void Detach<T, TKey>(T entity) where T : BaseEntity<TKey> where TKey : IEquatable<TKey>;
        void DetachRange<T, TKey>(IEnumerable<T> entities) where T : BaseEntity<TKey> where TKey : IEquatable<TKey>;
        void Attach<T, TKey>(T entity) where T : BaseEntity<TKey> where TKey : IEquatable<TKey>;
        void AttachRange<T, TKey>(IEnumerable<T> entities) where T : BaseEntity<TKey> where TKey : IEquatable<TKey>;

        // State Management
        void ResetState();
        Task EnsureDeletedAsync(CancellationToken cancellationToken = default);
    }
}
