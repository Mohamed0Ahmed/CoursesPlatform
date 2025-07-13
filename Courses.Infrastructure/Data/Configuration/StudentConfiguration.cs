using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Courses.Domain.Models;
using Courses.Domain.Identity;

namespace Courses.Infrastructure.Data.Configuration
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Students");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .HasDefaultValueSql("NEWID()");

            builder.Property(s => s.UserId)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(s => s.StudentNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(s => s.EnrollmentDate)
                .IsRequired();

            builder.Property(s => s.Bio)
                .HasMaxLength(2000);

            builder.Property(s => s.ProfilePictureUrl)
                .HasMaxLength(500);

            builder.Property(s => s.Country)
                .HasMaxLength(100);

            builder.Property(s => s.City)
                .HasMaxLength(100);

            // Indexes
            builder.HasIndex(s => s.UserId)
                .IsUnique()
                .HasDatabaseName("IX_Students_UserId");

            builder.HasIndex(s => s.StudentNumber)
                .IsUnique()
                .HasDatabaseName("IX_Students_StudentNumber");

            builder.HasIndex(s => s.EnrollmentDate)
                .HasDatabaseName("IX_Students_EnrollmentDate");

            // Foreign Key Relationship with ApplicationUser
            builder.HasOne(s => s.User)
                .WithOne(u => u.Student)
                .HasForeignKey<Student>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}