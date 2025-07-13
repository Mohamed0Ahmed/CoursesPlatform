

namespace Courses.Infrastructure.Data.Configuration
{
    public class CertificateConfiguration : IEntityTypeConfiguration<Certificate>
    {
        public void Configure(EntityTypeBuilder<Certificate> builder)
        {
            builder.ToTable("Certificates");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasDefaultValueSql("NEWID()");

            builder.Property(c => c.EnrollmentId)
                .IsRequired();

            builder.Property(c => c.IssueDate)
                .IsRequired();

            builder.Property(c => c.CertificateURL)
                .IsRequired()
                .HasMaxLength(500);

            // Indexes
            builder.HasIndex(c => c.EnrollmentId)
                .IsUnique()
                .HasDatabaseName("IX_Certificates_EnrollmentId");

            builder.HasIndex(c => c.IssueDate)
                .HasDatabaseName("IX_Certificates_IssueDate");

            // Foreign Key Relationship
            builder.HasOne<Enrollment>()
                .WithMany()
                .HasForeignKey(c => c.EnrollmentId)
                .OnDelete(DeleteBehavior.Cascade); // نحذف Certificate إذا حذف Enrollment
        }
    }
}