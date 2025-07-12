using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Courses.Domain.Models;

namespace Courses.Infrastructure.Data.Configuration
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Students");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .HasMaxLength(450);

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.Email)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(s => s.PasswordHash)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(s => s.CreatedDate)
                .IsRequired();

            builder.Property(s => s.UpdatedDate)
                .IsRequired();

            // Indexes
            builder.HasIndex(s => s.Email)
                .IsUnique()
                .HasDatabaseName("IX_Students_Email");

            builder.HasIndex(s => s.CreatedDate)
                .HasDatabaseName("IX_Students_CreatedDate");
        }
    }
}