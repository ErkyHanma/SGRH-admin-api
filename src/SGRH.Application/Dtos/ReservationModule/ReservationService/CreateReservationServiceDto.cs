namespace SGRH.Application.Dtos.ReservationModule.ReservationService
{
    public record class CreateReservationServiceDto
    {
        public int ReservationId { get; set; }
        public int ServiceId { get; set; }
    }
}
