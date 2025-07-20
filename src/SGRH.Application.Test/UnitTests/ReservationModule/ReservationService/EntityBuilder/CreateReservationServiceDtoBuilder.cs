using SGRH.Application.Dtos.ReservationModule.ReservationService;


namespace SGRH.Application.Test.Test.ReservationModule.ReservationService.EntityBuilder
{
    public class CreateReservationServiceDtoBuilder : BaseReservationServiceDtoBuilder<CreateReservationServiceDto>
    {
        public CreateReservationServiceDtoBuilder()
        {
            _dto = new CreateReservationServiceDto();
        }

        public override CreateReservationServiceDto Build()
        {
            return _dto;
        }


    }
}
