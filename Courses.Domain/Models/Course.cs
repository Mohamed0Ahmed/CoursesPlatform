using Courses.Shared.Base;

namespace Courses.Domain.Models
{
    public class Course : BaseEntity<Guid>
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid InstructorId { get; set; } // Foreign Key to Instructor
        public bool IsActive { get; set; } = true;
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public string? VideoUrl { get; set; }
        public string Level { get; set; } = string.Empty; // Beginner, Intermediate, Advanced
        public string Language { get; set; } = "Arabic";
        public int? EstimatedHours { get; set; }

        // Navigation Properties
        public Instructor Instructor { get; set; } = null!;
        public ICollection<Section> Sections { get; set; } = new List<Section>();
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
    }
}