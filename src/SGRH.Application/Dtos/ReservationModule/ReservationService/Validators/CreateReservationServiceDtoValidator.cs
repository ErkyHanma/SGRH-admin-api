using SGRH.Domain.Base;

namespace SGRH.Application.Dtos.ReservationModule.ReservationService.Validators
{
    public class CreateReservationServiceDtoValidator
    {
        public static OperationResult<CreateReservationServiceDto> Validate(CreateReservationServiceDto dto)
        {
            if (dto == null)
                return OperationResult<CreateReservationServiceDto>.Failure("Dto cannot be null.");

            if (dto.ReservationId <= 0)
                return OperationResult<CreateReservationServiceDto>.Failure("ReservationId must be greater than zero.");

            if (dto.ServiceId <= 0)
                return OperationResult<CreateReservationServiceDto>.Failure("ServiceId must be greater than zero.");

            return OperationResult<CreateReservationServiceDto>.Success("All fields validated! ", dto);
        }
    }
}
