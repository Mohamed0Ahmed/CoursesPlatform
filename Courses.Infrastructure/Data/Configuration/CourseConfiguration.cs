

namespace Courses.Infrastructure.Data.Configuration
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.ToTable("Courses");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasDefaultValueSql("NEWID()");

            builder.Property(c => c.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(c => c.Description)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(c => c.InstructorId)
                .IsRequired();

            builder.Property(c => c.IsActive)
                .HasDefaultValue(true);

            builder.Property(c => c.Price)
                .HasPrecision(18, 2)
                .HasDefaultValue(0);

            builder.Property(c => c.ImageUrl)
                .HasMaxLength(500);

            builder.Property(c => c.VideoUrl)
                .HasMaxLength(500);

            builder.Property(c => c.Level)
                .HasMaxLength(50)
                .HasDefaultValue("Beginner");

            builder.Property(c => c.Language)
                .HasMaxLength(50)
                .HasDefaultValue("Arabic");

            builder.Property(c => c.EstimatedHours)
                .HasDefaultValue(0);

            // Indexes
            builder.HasIndex(c => c.InstructorId)
                .HasDatabaseName("IX_Courses_InstructorId");

            builder.HasIndex(c => c.IsActive)
                .HasDatabaseName("IX_Courses_IsActive");

            builder.HasIndex(c => c.Title)
                .HasDatabaseName("IX_Courses_Title");

            builder.HasIndex(c => c.Level)
                .HasDatabaseName("IX_Courses_Level");

            builder.HasIndex(c => c.Price)
                .HasDatabaseName("IX_Courses_Price");

            // Foreign Key Relationship with Instructor
            builder.HasOne(c => c.Instructor)
                .WithMany(i => i.Courses)
                .HasForeignKey(c => c.InstructorId)
                .OnDelete(DeleteBehavior.Restrict); // لا نحذف Instructor إذا كان له Courses
        }
    }
}