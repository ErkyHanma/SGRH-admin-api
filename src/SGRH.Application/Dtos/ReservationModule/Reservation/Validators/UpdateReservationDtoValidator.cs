using SGRH.Domain.Base;

namespace SGRH.Application.Dtos.ReservationModule.Reservation.Validators
{
    public class UpdateReservationDtoValidator : BaseReservationDtoValidator<UpdateReservationDto>
    {
        public override OperationResult<UpdateReservationDto> Validate(UpdateReservationDto dto)
        {

            var result = base.Validate(dto);
            if (!result.IsSuccess)
                return result;

            if (dto.ReservationId <= 0)
                return OperationResult<UpdateReservationDto>.Failure("ReservationId must be greater than zero.");

            return OperationResult<UpdateReservationDto>.Success("All fields validated! ", dto);
        }

    }
}
