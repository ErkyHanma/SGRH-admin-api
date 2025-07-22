using SGRH.Domain.Base;

namespace SGRH.Application.Dtos.ReservationModule.Reservation.Validators
{
    public class DeleteReservationDtoValidator
    {
        public OperationResult<DeleteReservationDto> Validate(DeleteReservationDto dto)
        {
            if (dto == null)
                return OperationResult<DeleteReservationDto>.Failure("Dto cannot be null.");

            if (dto.ReservationId <= 0)
                return OperationResult<DeleteReservationDto>.Failure("ReservationId must be greater than zero.");



            return OperationResult<DeleteReservationDto>.Success("All fields validated! ", dto);
        }
    }
}


