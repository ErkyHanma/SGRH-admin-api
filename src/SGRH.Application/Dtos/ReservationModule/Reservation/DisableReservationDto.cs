namespace SGRH.Application.Dtos.ReservationModule.Reservation
{
    public record class DisableReservationDto
    {
        public int ReservationId { get; set; }

        public int UpdatedBy { get; set; }
        public int DeleteBy { get; set; }
    }
}

