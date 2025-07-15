using SGRH.Application.Dtos.ReservationModule.Reservation;

namespace SGRH.Application.Test.Test.ReservationModule.Reservation.EntityBuilder
{
    public class UpdateReservationDtoBuilder : BaseReservationDtoBuilder<UpdateReservationDto>
    {

        public UpdateReservationDtoBuilder()
        {
            _entity = new UpdateReservationDto();
        }

        public UpdateReservationDtoBuilder WithReservationId(int reservationId)
        {
            _entity.ReservationId = reservationId;
            return this;
        }

        public override UpdateReservationDto Build()
        {
            return _entity;
        }


        public override UpdateReservationDtoBuilder WithTestValues()
        {
            _entity.ReservationId = 1;
            _entity.ClientId = 3;
            _entity.RoomId = 3;
            _entity.StartDate = new DateTime(2055, 7, 10);
            _entity.EndDate = new DateTime(2055, 7, 11);
            _entity.Status = "Pending";
            _entity.GuestCount = 2;
            _entity.PaymentAmount = 100;

            return this;
        }
    }
}

