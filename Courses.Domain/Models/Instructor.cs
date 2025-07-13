
namespace Courses.Domain.Models
{
    public class Instructor : BaseEntity<Guid>
    {
        public string UserId { get; set; } = string.Empty; // Foreign Key to AspNetUsers
        public string Bio { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public decimal Rating { get; set; }
        public int YearsOfExperience { get; set; }
        public string Website { get; set; } = string.Empty;
        public string LinkedIn { get; set; } = string.Empty;

        // Navigation Properties
        public ApplicationUser User { get; set; } = null!;
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}