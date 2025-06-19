namespace SGRH.Application.Dtos.ReservationModule.ReservationService
{
    public record class DeleteReservationServiceDto
    {
        public int ReservationId { get; set; }
        public int ServiceId { get; set; }
    }
}
