using SGRH.Application.Dtos.ReservationModule.Reservation;

namespace SGRH.Application.Test.Test.ReservationModule.Reservation.EntityBuilder
{
    public class DeleteReservationDtoBuilder
    {

        private readonly DeleteReservationDto _dto;
        public DeleteReservationDtoBuilder()
        {
            _dto = new DeleteReservationDto();
        }

        public DeleteReservationDtoBuilder WithReservationId(int reservationId)
        {
            _dto.ReservationId = reservationId;
            return this;
        }

        public DeleteReservationDto Build()
        {
            return _dto;
        }


        public DeleteReservationDtoBuilder WithTestValues()
        {
            _dto.ReservationId = 1;

            return this;
        }
    }
}


