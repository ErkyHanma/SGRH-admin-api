using SGRH.Domain.Base;

namespace SGRH.Application.Dtos.ReservationModule.ReservationService.Validators
{
    public class DeleteReservationServiceDtoValidator : BaseReservationServiceDtoValidator<DeleteReservationServiceDto>
    {
        public override OperationResult<DeleteReservationServiceDto> Validate(DeleteReservationServiceDto dto)
        {
            var result = base.Validate(dto);
            if (!result.IsSuccess)
                return result;

            return OperationResult<DeleteReservationServiceDto>.Success("All fields validated! ", dto);
        }
    }
}
