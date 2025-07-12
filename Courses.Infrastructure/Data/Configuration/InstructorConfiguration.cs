using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Courses.Domain.Models;

namespace Courses.Infrastructure.Data.Configuration
{
    public class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
    {
        public void Configure(EntityTypeBuilder<Instructor> builder)
        {
            builder.ToTable("Instructors");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Id)
                .HasMaxLength(450);

            builder.Property(i => i.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(i => i.Email)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(i => i.PasswordHash)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(i => i.Bio)
                .HasMaxLength(1000);

            builder.Property(i => i.CreatedDate)
                .IsRequired();

            builder.Property(i => i.UpdatedDate)
                .IsRequired();

            // Indexes
            builder.HasIndex(i => i.Email)
                .IsUnique()
                .HasDatabaseName("IX_Instructors_Email");

            builder.HasIndex(i => i.CreatedDate)
                .HasDatabaseName("IX_Instructors_CreatedDate");
        }
    }
}