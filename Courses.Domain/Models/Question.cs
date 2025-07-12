using Courses.Shared.Base;

namespace Courses.Domain.Models
{
    public class Question : BaseEntity<Guid>
    {
        public Guid QuizId { get; set; }
        public string Text { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
    }
}