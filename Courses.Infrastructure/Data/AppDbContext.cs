
namespace Courses.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
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
                    // استخدام IEntity بدلاً من BaseEntity<Guid>
                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(IEntity.CreatedAt))
                        .HasDefaultValueSql("GETUTCDATE()");

                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(IEntity.UpdatedAt))
                        .HasDefaultValueSql("GETUTCDATE()");

                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(IEntity.LastModifiedOn))
                        .HasDefaultValueSql("GETUTCDATE()");

                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(IEntity.IsDeleted))
                        .HasDefaultValue(false);

                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(IEntity.CreatedBy))
                        .HasMaxLength(450);

                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(IEntity.UpdatedBy))
                        .HasMaxLength(450);

                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(IEntity.DeletedBy))
                        .HasMaxLength(450);

                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(IEntity.LastModifiedBy))
                        .HasMaxLength(450);

                    // Add indexes
                    modelBuilder.Entity(entityType.ClrType)
                        .HasIndex(nameof(IEntity.CreatedAt))
                        .HasDatabaseName($"IX_{entityType.GetTableName()}_CreatedAt");

                    modelBuilder.Entity(entityType.ClrType)
                        .HasIndex(nameof(IEntity.IsDeleted))
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
                    entity.CreatedAt = DateTime.UtcNow;
                    entity.UpdatedAt = DateTime.UtcNow;
                    entity.LastModifiedOn = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entity.UpdatedAt = DateTime.UtcNow;
                    entity.LastModifiedOn = DateTime.UtcNow;
                }
            }
        }
    }
}
