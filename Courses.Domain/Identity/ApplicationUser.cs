
namespace Courses.Domain.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public UserType UserType { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        // Navigation Properties
        public Student? Student { get; set; }
        public Instructor? Instructor { get; set; }

        // Full Name property
        public string FullName => $"{FirstName} {LastName}".Trim();
    }
}