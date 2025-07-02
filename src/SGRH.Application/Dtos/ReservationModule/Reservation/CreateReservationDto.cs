namespace SGRH.Application.Dtos.ReservationModule.Reservation
{
    public record class CreateReservationDto

    {
        public int ClientId { get; set; }
        public int RoomId { get; set; }
        public DateTime StartDate { get; set; } = new DateTime();
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public int GuestCount { get; set; }
        public decimal PaymentAmount { get; set; } = 0.00m;
        public int CreatedBy { get; set; }


    }
}


