using SGRH.Application.Dtos.ReservationModule.ReservationService;

namespace SGRH.Persistence.Test.Test.ReservationModule.ReservationService.EntityBuilder
{
    public abstract class BaseReservationServiceDtoBuilder<T> where T : BaseReservationServiceDto
    {

        public T _entity;

        public BaseReservationServiceDtoBuilder<T> WithReservationId(int reservationId)
        {
            _entity.ReservationId = reservationId;
            return this;
        }

        public BaseReservationServiceDtoBuilder<T> WithServiceId(int serviceId)
        {
            _entity.ServiceId = serviceId;
            return this;
        }


        public abstract T Build();
        public virtual BaseReservationServiceDtoBuilder<T> WithTestValues()
        {
            {
                _entity.ReservationId = 1;
                _entity.ServiceId = 1;

                return this;
            }
        }
    }
}


