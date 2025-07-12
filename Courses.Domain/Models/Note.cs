using Courses.Shared.Base;

namespace Courses.Domain.Models
{
    public class Note : BaseEntity<Guid>
    {
        public Guid SectionId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}