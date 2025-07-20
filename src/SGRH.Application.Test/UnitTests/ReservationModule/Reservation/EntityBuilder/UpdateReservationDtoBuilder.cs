using SGRH.Application.Dtos.ReservationModule.Reservation;

namespace SGRH.Application.Test.Test.ReservationModule.Reservation.EntityBuilder
{
    public class UpdateReservationDtoBuilder : BaseReservationDtoBuilder<UpdateReservationDto>
    {

        public UpdateReservationDtoBuilder()
        {
            _dto = new UpdateReservationDto();
        }

        public UpdateReservationDtoBuilder WithReservationId(int reservationId)
        {
            _dto.ReservationId = reservationId;
            return this;
        }

        public override UpdateReservationDto Build()
        {
            return _dto;
        }


        public override UpdateReservationDtoBuilder WithTestValues()
        {
            _dto.ReservationId = 1;
            _dto.ClientId = 3;
            _dto.RoomId = 3;
            _dto.StartDate = new DateTime(2055, 7, 10);
            _dto.EndDate = new DateTime(2055, 7, 11);
            _dto.Status = "Pending";
            _dto.GuestCount = 2;
            _dto.PaymentAmount = 100;

            return this;
        }
    }
}

