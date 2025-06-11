using SGRH.Domain.Base;

namespace SGRH.Domain.Entities.Services
{
    public class Service : AuditEntity
    {
        public int ServiceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}

