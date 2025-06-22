using SGRH.Domain.Base;

namespace SGRH.Application.Dtos.ReservationModule.Reservation.Validators
{
    public class DisableReservationDtoValidator
    {
        public static OperationResult<DisableReservationDto> Validate(DisableReservationDto dto)
        {
            if (dto == null)
                return OperationResult<DisableReservationDto>.Failure("Dto cannot be null.");

            if (dto.ReservationId <= 0)
                return OperationResult<DisableReservationDto>.Failure("ReservationId must be greater than zero.");

            if (dto.UpdatedBy <= 0)
                return OperationResult<DisableReservationDto>.Failure("UpdatedBy must be greater than zero.");

            if (dto.DeleteBy <= 0)
                return OperationResult<DisableReservationDto>.Failure("DeleteBy must be greater than zero.");

            return OperationResult<DisableReservationDto>.Success("All fields validated! ", dto);
        }
    }
}


