

namespace Courses.Infrastructure.Data.Configuration
{
    public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
    {
        public void Configure(EntityTypeBuilder<Enrollment> builder)
        {
            builder.ToTable("Enrollments");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasDefaultValueSql("NEWID()");

            builder.Property(e => e.StudentId)
                .IsRequired();

            builder.Property(e => e.CourseId)
                .IsRequired();

            builder.Property(e => e.EnrollmentDate)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(e => e.IsCompleted)
                .HasDefaultValue(false);

            builder.Property(e => e.Grade)
                .HasPrecision(5, 2);

            builder.Property(e => e.Progress)
                .HasPrecision(5, 2)
                .HasDefaultValue(0m);

            // Indexes
            builder.HasIndex(e => e.StudentId)
                .HasDatabaseName("IX_Enrollments_StudentId");

            builder.HasIndex(e => e.CourseId)
                .HasDatabaseName("IX_Enrollments_CourseId");

            builder.HasIndex(e => e.EnrollmentDate)
                .HasDatabaseName("IX_Enrollments_EnrollmentDate");

            builder.HasIndex(e => e.IsCompleted)
                .HasDatabaseName("IX_Enrollments_IsCompleted");

            // Composite Index for unique enrollment
            builder.HasIndex(e => new { e.StudentId, e.CourseId })
                .IsUnique()
                .HasDatabaseName("IX_Enrollments_StudentId_CourseId");

            // Foreign Key Relationships
            builder.HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}