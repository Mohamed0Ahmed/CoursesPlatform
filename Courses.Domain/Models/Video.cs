using Courses.Shared.Base;

namespace Courses.Domain.Models
{
    public class Video : BaseEntity<Guid>
    {
        public Guid SectionId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string URL { get; set; } = string.Empty;
        public int Duration { get; set; }
    }
}