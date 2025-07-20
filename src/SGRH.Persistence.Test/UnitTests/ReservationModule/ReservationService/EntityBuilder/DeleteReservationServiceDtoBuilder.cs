using SGRH.Application.Dtos.ReservationModule.ReservationService;

namespace SGRH.Persistence.Test.Test.ReservationModule.ReservationService.EntityBuilder
{
    public class DeleteReservationServiceDtoBuilder : BaseReservationServiceDtoBuilder<DeleteReservationServiceDto>
    {
        public DeleteReservationServiceDtoBuilder()
        {
            _entity = new DeleteReservationServiceDto();
        }

        public override DeleteReservationServiceDto Build()
        {
            return _entity;
        }

    }
}
