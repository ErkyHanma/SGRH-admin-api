using SGRH.Application.Dtos.ReservationModule.Reservation;

namespace SGRH.Persistence.Test.Test.ReservationModule.Reservation.EntityBuilder
{
    public class ReservationDtoBuilder : BaseReservationDtoBuilder<ReservationDto>
    {
        public ReservationDtoBuilder()
        {
            _entity = new ReservationDto();
        }

        public ReservationDtoBuilder WithReservationId(int reservationId)
        {
            _entity.ReservationId = reservationId;
            return this;
        }

        public override ReservationDto Build()
        {
            return _entity;
        }


        public override ReservationDtoBuilder WithTestValues()
        {
            _entity.ReservationId = 1;
            _entity.ClientId = 3;
            _entity.RoomId = 3;
            _entity.StartDate = new DateTime(2026, 7, 10);
            _entity.EndDate = new DateTime(2026, 7, 11);
            _entity.Status = "Pending";
            _entity.GuestCount = 2;
            _entity.PaymentAmount = 100;
            _entity.ReservationDate = DateTime.Now;
            _entity.ServicesCount = 3;
            _entity.TotalServicesCost = 100;
            _entity.ServiceNames = "Spa, Limpiar, Extra Bed";
            _entity.CreatedAt = new DateTime(2025, 7, 13);
            _entity.UpdatedAt = new DateTime(2025, 7, 14);

            return this;
        }
    }
}
