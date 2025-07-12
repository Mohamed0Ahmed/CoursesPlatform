using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Courses.Domain.Models;
using Courses.Shared.Base;

namespace Courses.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSets for all entities
        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Certificate> Certificates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply all entity configurations

            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);


            // Configure BaseEntity properties for all entities
            ConfigureBaseEntityProperties(modelBuilder);
        }

        private void ConfigureBaseEntityProperties(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(IEntity).IsAssignableFrom(entityType.ClrType))
                {
                    // Configure common BaseEntity properties
                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(BaseEntity<Guid>.CreatedAt))
                        .HasDefaultValueSql("GETUTCDATE()");

                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(BaseEntity<Guid>.UpdatedAt))
                        .HasDefaultValueSql("GETUTCDATE()");

                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(BaseEntity<Guid>.LastModifiedOn))
                        .HasDefaultValueSql("GETUTCDATE()");

                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(BaseEntity<Guid>.IsDeleted))
                        .HasDefaultValue(false);

                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(BaseEntity<Guid>.CreatedBy))
                        .HasMaxLength(450);

                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(BaseEntity<Guid>.UpdatedBy))
                        .HasMaxLength(450);

                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(BaseEntity<Guid>.DeletedBy))
                        .HasMaxLength(450);

                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(BaseEntity<Guid>.LastModifiedBy))
                        .HasMaxLength(450);

                    // Add indexes for common BaseEntity properties
                    modelBuilder.Entity(entityType.ClrType)
                        .HasIndex(nameof(BaseEntity<Guid>.CreatedAt))
                        .HasDatabaseName($"IX_{entityType.GetTableName()}_CreatedAt");

                    modelBuilder.Entity(entityType.ClrType)
                        .HasIndex(nameof(BaseEntity<Guid>.IsDeleted))
                        .HasDatabaseName($"IX_{entityType.GetTableName()}_IsDeleted");
                }
            }
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is IEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var entity = (IEntity)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    if (entity is BaseEntity<Guid> guidEntity)
                    {
                        guidEntity.CreatedAt = DateTime.UtcNow;
                        guidEntity.UpdatedAt = DateTime.UtcNow;
                        guidEntity.LastModifiedOn = DateTime.UtcNow;
                    }
                    else if (entity is BaseEntity<string> stringEntity)
                    {
                        stringEntity.CreatedAt = DateTime.UtcNow;
                        stringEntity.UpdatedAt = DateTime.UtcNow;
                        stringEntity.LastModifiedOn = DateTime.UtcNow;
                    }
                }
                else if (entry.State == EntityState.Modified)
                {
                    if (entity is BaseEntity<Guid> guidEntity)
                    {
                        guidEntity.UpdatedAt = DateTime.UtcNow;
                        guidEntity.LastModifiedOn = DateTime.UtcNow;
                    }
                    else if (entity is BaseEntity<string> stringEntity)
                    {
                        stringEntity.UpdatedAt = DateTime.UtcNow;
                        stringEntity.LastModifiedOn = DateTime.UtcNow;
                    }
                }
            }
        }
    }
}
