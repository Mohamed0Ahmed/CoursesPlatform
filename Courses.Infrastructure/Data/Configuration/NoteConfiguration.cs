using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Courses.Domain.Models;

namespace Courses.Infrastructure.Data.Configuration
{
    public class NoteConfiguration : IEntityTypeConfiguration<Note>
    {
        public void Configure(EntityTypeBuilder<Note> builder)
        {
            builder.ToTable("Notes");

            builder.HasKey(n => n.Id);

            builder.Property(n => n.Id)
                .HasDefaultValueSql("NEWID()");

            builder.Property(n => n.SectionId)
                .IsRequired();

            builder.Property(n => n.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(n => n.Content)
                .IsRequired()
                .HasMaxLength(5000);

            builder.Property(n => n.CreatedDate)
                .IsRequired();

            builder.Property(n => n.UpdatedDate)
                .IsRequired();

            // Indexes
            builder.HasIndex(n => n.SectionId)
                .HasDatabaseName("IX_Notes_SectionId");

            builder.HasIndex(n => n.CreatedDate)
                .HasDatabaseName("IX_Notes_CreatedDate");

            // Foreign Key Relationship
            builder.HasOne<Section>()
                .WithMany()
                .HasForeignKey(n => n.SectionId)
                .OnDelete(DeleteBehavior.Cascade); // نحذف Notes إذا حذف Section
        }
    }
}