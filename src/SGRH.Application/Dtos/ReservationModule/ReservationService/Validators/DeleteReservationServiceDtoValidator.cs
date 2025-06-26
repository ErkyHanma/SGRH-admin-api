using SGRH.Domain.Base;

namespace SGRH.Application.Dtos.ReservationModule.ReservationService.Validators
{
    public class DeleteReservationServiceDtoValidator
    {
        public static OperationResult<DeleteReservationServiceDto> Validate(DeleteReservationServiceDto dto)
        {
            if (dto == null)
                return OperationResult<DeleteReservationServiceDto>.Failure("Dto cannot be null.");

            if (dto.ReservationId <= 0)
                return OperationResult<DeleteReservationServiceDto>.Failure("ReservationId must be greater than zero.");

            if (dto.ServiceId <= 0)
                return OperationResult<DeleteReservationServiceDto>.Failure("ServiceId must be greater than zero.");

            return OperationResult<DeleteReservationServiceDto>.Success("All fields validated! ", dto);
        }
    }
}
