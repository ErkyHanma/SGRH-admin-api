using SGRH.Domain.Base;

namespace SGRH.Domain.Entities.Hotel
{
    public class Season : AuditEntity
    {
        public int SeasonId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
