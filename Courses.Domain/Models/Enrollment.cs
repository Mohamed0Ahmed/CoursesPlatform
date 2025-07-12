using Courses.Shared.Base;

namespace Courses.Domain.Models
{
    public class Enrollment : BaseEntity<Guid>
    {
        public string StudentId { get; set; } = string.Empty;
        public Guid CourseId { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletionDate { get; set; }
        public decimal? Grade { get; set; }
    }
}