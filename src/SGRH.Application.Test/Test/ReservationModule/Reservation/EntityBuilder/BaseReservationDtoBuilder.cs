using SGRH.Application.Dtos.ReservationModule.Reservation;

namespace SGRH.Application.Test.Test.ReservationModule.Reservation.EntityBuilder
{
    public abstract class BaseReservationDtoBuilder<T> where T : BaseReservationDto
    {
        public const int TEST_SERVICE_ID = 2;
        public T _dto;

        public BaseReservationDtoBuilder<T> WithClientId(int clientId)
        {
            _dto.ClientId = clientId;
            return this;
        }

        public BaseReservationDtoBuilder<T> WithRoomId(int roomId)
        {
            _dto.RoomId = roomId;
            return this;
        }

        public BaseReservationDtoBuilder<T> WithStartDate(DateTime startDate)
        {
            _dto.StartDate = startDate;
            return this;
        }

        public BaseReservationDtoBuilder<T> WithEndDate(DateTime endDate)
        {
            _dto.EndDate = endDate;
            return this;
        }

        public BaseReservationDtoBuilder<T> WithStatus(string status)
        {
            _dto.Status = status;
            return this;
        }

        public BaseReservationDtoBuilder<T> WithGuestCount(int guestCount)
        {
            _dto.GuestCount = guestCount;
            return this;
        }

        public BaseReservationDtoBuilder<T> WithPaymentAmount(int paymentAmount)
        {
            _dto.PaymentAmount = paymentAmount;
            return this;
        }

        public abstract T Build();
        public abstract BaseReservationDtoBuilder<T> WithTestValues();


    }
}

