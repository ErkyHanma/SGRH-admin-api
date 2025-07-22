using SGRH.Application.Dtos.ReservationModule.ReservationService;

namespace SGRH.Application.Test.Test.ReservationModule.ReservationService.EntityBuilder
{
    public class DeleteReservationServiceDtoBuilder : BaseReservationServiceDtoBuilder<DeleteReservationServiceDto>
    {
        public DeleteReservationServiceDtoBuilder()
        {
            _dto = new DeleteReservationServiceDto();
        }

        public override DeleteReservationServiceDto Build()
        {
            return _dto;
        }

    }
}
