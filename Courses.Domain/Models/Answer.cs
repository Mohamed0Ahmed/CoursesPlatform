using Courses.Shared.Base;

namespace Courses.Domain.Models
{
    public class Answer : BaseEntity<Guid>
    {
        public Guid QuestionId { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
    }
}