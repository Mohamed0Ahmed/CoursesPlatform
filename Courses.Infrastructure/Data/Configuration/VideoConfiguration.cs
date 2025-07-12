using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Courses.Domain.Models;

namespace Courses.Infrastructure.Data.Configuration
{
    public class VideoConfiguration : IEntityTypeConfiguration<Video>
    {
        public void Configure(EntityTypeBuilder<Video> builder)
        {
            builder.HasKey(v => v.Id);
            builder.Property(v => v.Title).IsRequired().HasMaxLength(200);
            builder.Property(v => v.URL).IsRequired();
            builder.Property(v => v.Duration).IsRequired();
            builder.Property(v => v.Order).IsRequired();

            builder.HasIndex(v => new { v.SectionId, v.Order });
        }
    }
}