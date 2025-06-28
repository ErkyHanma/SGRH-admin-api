namespace SGRH.Domain.Base
{
    public abstract class AuditEntity
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeleteAt { get; set; } // <----- there's a typo
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
