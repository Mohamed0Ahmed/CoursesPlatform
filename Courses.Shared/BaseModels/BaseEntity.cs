namespace Courses.Shared.BaseModels
{     public class BaseEntity<TKey> : IEntity where TKey : IEquatable<TKey>
    {
        public TKey Id { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; } = string.Empty; 
        public string UpdatedBy { get; set; } = string.Empty; 
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime LastModifiedOn { get; set; } 
        public string? LastModifiedBy { get; set; }
    }
}
