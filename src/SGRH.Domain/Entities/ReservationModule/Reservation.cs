using SGRH.Domain.Base;

namespace SGRH.Domain.Entities.ReservationModule
{
    public class Reservation : AuditEntity
    {
        public int ReservationId { get; set; }
        public int ClientId { get; set; }
        public int RoomId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ReservationDate { get; set; } = DateTime.UtcNow;
        public string? Status { get; set; }
        public int GuestCount { get; set; }
        public decimal PaymentAmount { get; set; }
    }

}
