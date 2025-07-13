using Courses.Shared.Base;
using Courses.Domain.Identity;

namespace Courses.Domain.Models
{
    public class Student : BaseEntity<Guid>
    {
        public string UserId { get; set; } = string.Empty; // Foreign Key to AspNetUsers
        public string StudentNumber { get; set; } = string.Empty;
        public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;
        public string? Bio { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public DateTime? DateOfBirth { get; set; }

        // Navigation Properties
        public ApplicationUser User { get; set; } = null!;
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<Note> Notes { get; set; } = new List<Note>();
        public ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();
    }
}