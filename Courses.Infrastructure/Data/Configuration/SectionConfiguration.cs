using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Courses.Domain.Models;

namespace Courses.Infrastructure.Data.Configuration
{
    public class SectionConfiguration : IEntityTypeConfiguration<Section>
    {
        public void Configure(EntityTypeBuilder<Section> builder)
        {
            builder.ToTable("Sections");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .HasDefaultValueSql("NEWID()");

            builder.Property(s => s.CourseId)
                .IsRequired();

            builder.Property(s => s.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(s => s.Order)
                .IsRequired();

            // Indexes
            builder.HasIndex(s => s.CourseId)
                .HasDatabaseName("IX_Sections_CourseId");

            builder.HasIndex(s => new { s.CourseId, s.Order })
                .IsUnique()
                .HasDatabaseName("IX_Sections_Course_Order");

            // Foreign Key Relationship
            builder.HasOne<Course>()
                .WithMany()
                .HasForeignKey(s => s.CourseId)
                .OnDelete(DeleteBehavior.Cascade); // نحذف Sections إذا حذف Course
        }
    }
}