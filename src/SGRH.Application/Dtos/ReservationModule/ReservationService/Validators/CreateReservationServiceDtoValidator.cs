using SGRH.Domain.Base;

namespace SGRH.Application.Dtos.ReservationModule.ReservationService.Validators
{
    public class CreateReservationServiceDtoValidator : BaseReservationServiceDtoValidator<CreateReservationServiceDto>
    {
        public override OperationResult<CreateReservationServiceDto> Validate(CreateReservationServiceDto dto)
        {
            var result = base.Validate(dto);
            if (!result.IsSuccess)
                return result;

            return OperationResult<CreateReservationServiceDto>.Success("All fields validated! ", dto);
        }
    }
}
