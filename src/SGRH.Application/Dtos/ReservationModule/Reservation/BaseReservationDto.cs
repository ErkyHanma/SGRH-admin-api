namespace SGRH.Application.Dtos.ReservationModule.Reservation
{
    public abstract class BaseReservationDto
    {
        public int ClientId { get; set; }
        public int RoomId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public int GuestCount { get; set; }
        public decimal PaymentAmount { get; set; }
    }
}
