namespace SGRH.Application.Dtos.ReservationModule.Reservation
{
    public class ReservationDto
    {
        public int ReservationId { get; set; }
        public int ClientId { get; set; }
        public int RoomId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ReservationDate { get; set; }
        public string Status { get; set; }
        public int GuestCount { get; set; }
        public decimal PaymentAmount { get; set; }
        public int ServicesCount { get; set; } = 0;
        public decimal TotalServicesCost { get; set; } = decimal.Zero;
        public string ServiceNames { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

}
