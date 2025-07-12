using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Courses.Domain.Models;

namespace Courses.Infrastructure.Data.Configuration
{
    public class VideoConfiguration : IEntityTypeConfiguration<Video>
    {
        public void Configure(EntityTypeBuilder<Video> builder)
        {
            builder.ToTable("Videos");

            builder.HasKey(v => v.Id);

            builder.Property(v => v.Id)
                .HasDefaultValueSql("NEWID()");

            builder.Property(v => v.SectionId)
                .IsRequired();

            builder.Property(v => v.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(v => v.URL)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(v => v.Duration)
                .IsRequired();

            // Indexes
            builder.HasIndex(v => v.SectionId)
                .HasDatabaseName("IX_Videos_SectionId");

            // Foreign Key Relationship
            builder.HasOne<Section>()
                .WithMany()
                .HasForeignKey(v => v.SectionId)
                .OnDelete(DeleteBehavior.Cascade); // نحذف Videos إذا حذف Section
        }
    }
}