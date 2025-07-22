using SGRH.Application.Dtos.ReservationModule.ReservationService;

namespace SGRH.Application.Test.Test.ReservationModule.ReservationService.EntityBuilder
{
    public abstract class BaseReservationServiceDtoBuilder<T> where T : BaseReservationServiceDto
    {

        public T _dto;

        public BaseReservationServiceDtoBuilder<T> WithReservationId(int reservationId)
        {
            _dto.ReservationId = reservationId;
            return this;
        }

        public BaseReservationServiceDtoBuilder<T> WithServiceId(int serviceId)
        {
            _dto.ServiceId = serviceId;
            return this;
        }


        public abstract T Build();
        public virtual BaseReservationServiceDtoBuilder<T> WithTestValues()
        {
            {
                _dto.ReservationId = 1;
                _dto.ServiceId = 1;

                return this;
            }
        }
    }
}


