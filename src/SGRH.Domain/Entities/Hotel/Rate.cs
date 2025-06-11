using SGRH.Domain.Base;

namespace SGRH.Domain.Entities.Hotel
{
    public class Rate : AuditEntity
    {
        public int RateId { get; set; }
        public int CategoryId { get; set; }
        public int SeasonId { get; set; }
        public decimal NightPrice { get; set; }

    }
}

