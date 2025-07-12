using SGRH.Domain.Base;

namespace SGRH.Application.Dtos.ReservationModule.Reservation.Validators
{
    public class CreateReservationDtoValidator : BaseReservationDtoValidator<CreateReservationDto>
    {
        public override OperationResult<CreateReservationDto> Validate(CreateReservationDto dto)
        {

            var result = base.Validate(dto);
            if (!result.IsSuccess)
                return result;



            return OperationResult<CreateReservationDto>.Success("All field validated", dto);
        }
    }
}
