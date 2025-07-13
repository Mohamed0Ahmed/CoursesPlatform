
namespace Courses.Domain.Models
{
    public class Certificate : BaseEntity<Guid>
    {
        public Guid EnrollmentId { get; set; }
        public DateTime IssueDate { get; set; }
        public string CertificateURL { get; set; } = string.Empty;
    }
}