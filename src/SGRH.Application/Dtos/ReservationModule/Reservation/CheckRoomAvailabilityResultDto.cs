namespace SGRH.Application.Dtos.ReservationModule.Reservation
{
    public record class CheckRoomAvailabilityResultDto
    {
        public bool IsAvailable { get; set; }
        public string Message { get; set; } = string.Empty;
    }

}
