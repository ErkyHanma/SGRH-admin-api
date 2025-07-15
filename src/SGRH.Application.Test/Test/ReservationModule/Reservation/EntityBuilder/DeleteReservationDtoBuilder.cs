using SGRH.Application.Dtos.ReservationModule.Reservation;

namespace SGRH.Application.Test.Test.ReservationModule.Reservation.EntityBuilder
{
    public class DeleteReservationDtoBuilder
    {

        private readonly DeleteReservationDto _entity;
        public DeleteReservationDtoBuilder()
        {
            _entity = new DeleteReservationDto();
        }

        public DeleteReservationDtoBuilder WithReservationId(int reservationId)
        {
            _entity.ReservationId = reservationId;
            return this;
        }

        public DeleteReservationDto Build()
        {
            return _entity;
        }


        public DeleteReservationDtoBuilder WithTestValues()
        {
            _entity.ReservationId = 1;

            return this;
        }
    }
}


