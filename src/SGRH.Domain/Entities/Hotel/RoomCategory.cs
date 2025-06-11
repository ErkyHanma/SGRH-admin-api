using SGRH.Domain.Base;

namespace SGRH.Domain.Entities.Hotel
{
    public class RoomCategory : AuditEntity
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MaxCapacity { get; set; }
        public string Amenities { get; set; }
    }
}

