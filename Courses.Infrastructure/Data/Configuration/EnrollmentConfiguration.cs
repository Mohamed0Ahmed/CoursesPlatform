using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Courses.Domain.Models;

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
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(e => e.CourseId)
                .IsRequired();

            builder.Property(e => e.EnrollmentDate)
                .IsRequired();

            builder.Property(e => e.IsCompleted)
                .HasDefaultValue(false);

            builder.Property(e => e.CompletionDate)
                .IsRequired(false);

            builder.Property(e => e.Grade)
                .HasColumnType("decimal(5,2)")
                .IsRequired(false);

            // Indexes
            builder.HasIndex(e => e.StudentId)
                .HasDatabaseName("IX_Enrollments_StudentId");

            builder.HasIndex(e => e.CourseId)
                .HasDatabaseName("IX_Enrollments_CourseId");

            builder.HasIndex(e => new { e.StudentId, e.CourseId })
                .IsUnique()
                .HasDatabaseName("IX_Enrollments_Student_Course");

            builder.HasIndex(e => e.EnrollmentDate)
                .HasDatabaseName("IX_Enrollments_EnrollmentDate");

            builder.HasIndex(e => e.IsCompleted)
                .HasDatabaseName("IX_Enrollments_IsCompleted");

            // Foreign Key Relationships
            builder.HasOne<Course>()
                .WithMany()
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade); // نحذف Enrollment إذا حذف Course

            builder.HasOne<Student>()
                .WithMany()
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Cascade); // نحذف Enrollment إذا حذف Student
        }
    }
}