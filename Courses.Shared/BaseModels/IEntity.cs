namespace Courses.Shared.BaseModels
{
    public interface IEntity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        DateTime LastModifiedOn { get; set; }
        string? LastModifiedBy { get; set; }
    }
}