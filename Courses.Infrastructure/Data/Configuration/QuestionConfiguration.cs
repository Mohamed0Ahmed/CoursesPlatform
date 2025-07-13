

namespace Courses.Infrastructure.Data.Configuration
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.ToTable("Questions");

            builder.HasKey(q => q.Id);

            builder.Property(q => q.Id)
                .HasDefaultValueSql("NEWID()");

            builder.Property(q => q.QuizId)
                .IsRequired();

            builder.Property(q => q.Text)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(q => q.Type)
                .IsRequired()
                .HasMaxLength(50);

            // Indexes
            builder.HasIndex(q => q.QuizId)
                .HasDatabaseName("IX_Questions_QuizId");

            builder.HasIndex(q => q.Type)
                .HasDatabaseName("IX_Questions_Type");

            // Foreign Key Relationship
            builder.HasOne<Quiz>()
                .WithMany()
                .HasForeignKey(q => q.QuizId)
                .OnDelete(DeleteBehavior.Cascade); // نحذف Questions إذا حذف Quiz
        }
    }
}