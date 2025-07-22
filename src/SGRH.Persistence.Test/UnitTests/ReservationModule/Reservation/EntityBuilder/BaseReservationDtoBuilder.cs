using SGRH.Application.Dtos.ReservationModule.Reservation;

namespace SGRH.Persistence.Test.Test.ReservationModule.Reservation.EntityBuilder
{
    public abstract class BaseReservationDtoBuilder<T> where T : BaseReservationDto
    {
        public const int TEST_SERVICE_ID = 2;
        public T _entity;

        public BaseReservationDtoBuilder<T> WithClientId(int clientId)
        {
            _entity.ClientId = clientId;
            return this;
        }

        public BaseReservationDtoBuilder<T> WithRoomId(int roomId)
        {
            _entity.RoomId = roomId;
            return this;
        }

        public BaseReservationDtoBuilder<T> WithStartDate(DateTime startDate)
        {
            _entity.StartDate = startDate;
            return this;
        }

        public BaseReservationDtoBuilder<T> WithEndDate(DateTime endDate)
        {
            _entity.EndDate = endDate;
            return this;
        }

        public BaseReservationDtoBuilder<T> WithStatus(string status)
        {
            _entity.Status = status;
            return this;
        }

        public BaseReservationDtoBuilder<T> WithGuestCount(int guestCount)
        {
            _entity.GuestCount = guestCount;
            return this;
        }

        public BaseReservationDtoBuilder<T> WithPaymentAmount(int paymentAmount)
        {
            _entity.PaymentAmount = paymentAmount;
            return this;
        }

        public abstract T Build();
        public abstract BaseReservationDtoBuilder<T> WithTestValues();


    }
}

