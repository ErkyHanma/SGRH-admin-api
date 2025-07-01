namespace SGRH.Application.Dtos.ReservationModule.Reservation
{
    public class UpdateReservationDto : BaseReservationDto
    {
        public int ReservationId { get; set; }
        public int UpdatedBy { get; set; }
    }
}

