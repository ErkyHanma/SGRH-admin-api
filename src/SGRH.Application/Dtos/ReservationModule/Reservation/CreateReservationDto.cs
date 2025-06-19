namespace SGRH.Application.Dtos.ReservationModule.Reservation
{
    public record class CreateReservationDto

    {
        public int ClientId { get; set; }
        public int RoomId { get; set; }
        public DateOnly StartDate { get; set; } = new DateOnly();
        public DateOnly EndDate { get; set; }
        public string Status { get; set; }
        public int GuestCount { get; set; }
        public int CreatedBy { get; set; }


    }
}


