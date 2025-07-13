

namespace Courses.Infrastructure.Data.Configuration
{
    public class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
    {
        public void Configure(EntityTypeBuilder<Instructor> builder)
        {
            builder.ToTable("Instructors");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Id)
                .HasDefaultValueSql("NEWID()");

            builder.Property(i => i.UserId)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(i => i.Bio)
                .HasMaxLength(2000);

            builder.Property(i => i.Specialization)
                .HasMaxLength(200);

            builder.Property(i => i.Rating)
                .HasPrecision(3, 2)
                .HasDefaultValue(0);

            builder.Property(i => i.YearsOfExperience)
                .HasDefaultValue(0);

            builder.Property(i => i.Website)
                .HasMaxLength(500);

            builder.Property(i => i.LinkedIn)
                .HasMaxLength(500);

            // Indexes
            builder.HasIndex(i => i.UserId)
                .IsUnique()
                .HasDatabaseName("IX_Instructors_UserId");

            builder.HasIndex(i => i.Rating)
                .HasDatabaseName("IX_Instructors_Rating");

            builder.HasIndex(i => i.Specialization)
                .HasDatabaseName("IX_Instructors_Specialization");

            // Foreign Key Relationship with ApplicationUser
            builder.HasOne(i => i.User)
                .WithOne(u => u.Instructor)
                .HasForeignKey<Instructor>(i => i.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}