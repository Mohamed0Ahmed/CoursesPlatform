
namespace Courses.Domain.Models
{
    public class Enrollment : BaseEntity<Guid>
    {
        public Guid StudentId { get; set; } // Foreign Key to Student
        public Guid CourseId { get; set; } // Foreign Key to Course
        public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;
        public bool IsCompleted { get; set; } = false;
        public DateTime? CompletionDate { get; set; }
        public decimal? Grade { get; set; }
        public decimal? Progress { get; set; } = 0; // Progress percentage (0-100)

        // Navigation Properties
        public Student Student { get; set; } = null!;
        public Course Course { get; set; } = null!;
    }
}