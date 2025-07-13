using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Courses.Domain.Models;

namespace Courses.Infrastructure.Data.Configuration
{
    public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
    {
        public void Configure(EntityTypeBuilder<Quiz> builder)
        {
            builder.ToTable("Quizzes");

            builder.HasKey(q => q.Id);

            builder.Property(q => q.Id)
                .HasDefaultValueSql("NEWID()");

            builder.Property(q => q.CourseId)
                .IsRequired();

            builder.Property(q => q.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(q => q.PassingScore)
                .IsRequired();

            // Indexes
            builder.HasIndex(q => q.CourseId)
                .HasDatabaseName("IX_Quizzes_CourseId");

            // Foreign Key Relationship
            builder.HasOne(q => q.Course)
                .WithMany(c => c.Quizzes)
                .HasForeignKey(q => q.CourseId)
                .OnDelete(DeleteBehavior.Cascade); // نحذف Quizzes إذا حذف Course
        }
    }
}