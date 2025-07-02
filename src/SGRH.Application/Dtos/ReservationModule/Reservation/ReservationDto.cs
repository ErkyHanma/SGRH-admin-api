namespace SGRH.Application.Dtos.ReservationModule.Reservation
{
    public class ReservationDto : BaseReservationDto
    {
        public int ReservationId { get; set; }
        public DateTime ReservationDate { get; set; }
        public int ServicesCount { get; set; } = 0;
        public decimal TotalServicesCost { get; set; } = decimal.Zero;
        public string ServiceNames { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

}

