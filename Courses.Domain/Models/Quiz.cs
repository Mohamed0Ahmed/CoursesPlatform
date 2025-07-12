using Courses.Shared.Base;

namespace Courses.Domain.Models
{
    public class Quiz : BaseEntity<Guid>
    {
        public Guid CourseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int PassingScore { get; set; }
    }
}