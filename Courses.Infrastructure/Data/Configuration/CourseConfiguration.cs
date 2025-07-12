using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Courses.Domain.Models;

namespace Courses.Infrastructure.Data.Configuration
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.ToTable("Courses");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasDefaultValueSql("NEWID()");

            builder.Property(c => c.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(c => c.Description)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(c => c.InstructorId)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(c => c.IsActive)
                .HasDefaultValue(true);

            builder.Property(c => c.CreatedDate)
                .IsRequired();

            builder.Property(c => c.UpdatedDate)
                .IsRequired();

            // Indexes
            builder.HasIndex(c => c.InstructorId)
                .HasDatabaseName("IX_Courses_InstructorId");

            builder.HasIndex(c => c.IsActive)
                .HasDatabaseName("IX_Courses_IsActive");

            builder.HasIndex(c => c.CreatedDate)
                .HasDatabaseName("IX_Courses_CreatedDate");

            // Foreign Key Relationship
            builder.HasOne<Instructor>()
                .WithMany()
                .HasForeignKey(c => c.InstructorId)
                .OnDelete(DeleteBehavior.Restrict); // لا نحذف Instructor إذا كان له Courses
        }
    }
}