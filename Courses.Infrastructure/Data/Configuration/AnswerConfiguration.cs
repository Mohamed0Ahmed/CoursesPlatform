

namespace Courses.Infrastructure.Data.Configuration
{
    public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder.ToTable("Answers");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                .HasDefaultValueSql("NEWID()");

            builder.Property(a => a.QuestionId)
                .IsRequired();

            builder.Property(a => a.Text)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(a => a.IsCorrect)
                .HasDefaultValue(false);

            // Indexes
            builder.HasIndex(a => a.QuestionId)
                .HasDatabaseName("IX_Answers_QuestionId");

            builder.HasIndex(a => new { a.QuestionId, a.IsCorrect })
                .HasDatabaseName("IX_Answers_Question_IsCorrect");

            // Foreign Key Relationship
            builder.HasOne<Question>()
                .WithMany()
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Cascade); // نحذف Answers إذا حذف Question
        }
    }
}