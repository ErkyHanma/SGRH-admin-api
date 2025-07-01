namespace SGRH.Application.Dtos.ReservationModule.Reservation
{
    public record class UpdateReservationDto
    {
        public int ReservationId { get; set; }
        public int ClientId { get; set; }
        public int RoomId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public int GuestCount { get; set; }
        public decimal PaymentAmount { get; set; }
        public int UpdatedBy { get; set; }
    }
}

