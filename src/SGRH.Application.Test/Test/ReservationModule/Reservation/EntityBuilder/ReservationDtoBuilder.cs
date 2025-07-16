using SGRH.Application.Dtos.ReservationModule.Reservation;

namespace SGRH.Application.Test.Test.ReservationModule.Reservation.EntityBuilder
{
    public class ReservationDtoBuilder : BaseReservationDtoBuilder<ReservationDto>
    {
        public ReservationDtoBuilder()
        {
            _dto = new ReservationDto();
        }

        public ReservationDtoBuilder WithReservationId(int reservationId)
        {
            _dto.ReservationId = reservationId;
            return this;
        }

        public override ReservationDto Build()
        {
            return _dto;
        }


        public override ReservationDtoBuilder WithTestValues()
        {
            _dto.ReservationId = 1;
            _dto.ClientId = 3;
            _dto.RoomId = 3;
            _dto.StartDate = new DateTime(2026, 7, 10);
            _dto.EndDate = new DateTime(2026, 7, 11);
            _dto.Status = "Pending";
            _dto.GuestCount = 2;
            _dto.PaymentAmount = 100;
            _dto.ReservationDate = DateTime.Now;
            _dto.ServicesCount = 3;
            _dto.TotalServicesCost = 100;
            _dto.ServiceNames = "Spa, Limpiar, Extra Bed";
            _dto.CreatedAt = new DateTime(2025, 7, 13);
            _dto.UpdatedAt = new DateTime(2025, 7, 14);

            return this;
        }
    }
}
